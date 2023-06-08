using Microsoft.AspNetCore.Mvc;
using MvcConciertosExam.Models;
using MvcConciertosExam.Services;

namespace MvcConciertosExam.Controllers {
    public class ConciertosController : Controller {

        private ServiceConciertos service;
        private ServiceStorageS3 serviceS3;
        private IConfiguration configuration;

        public ConciertosController(ServiceConciertos service
            , ServiceStorageS3 serviceS3, IConfiguration configuration) {
            this.service = service;
            this.configuration = configuration;
            this.serviceS3 = serviceS3;
        }

        public async Task<IActionResult> Index() {
            List<Evento> eventos = await this.service.GetEventosAsync();
            List<Categoria> categorias = await this.service.GetCategoriasAsync();
            ViewData["CATEGORIAS"] = categorias;
            foreach (var e in eventos) {
                e.Imagen = this.configuration.GetValue<string>("AWS:BucketUrl") + e.Imagen;
            }

            return View(eventos);
        }

        public async Task<IActionResult> EventosCategoria(int id) {
            List<Evento> eventos = await this.service.GetEventosCategoriaAsync(id);
            foreach (var e in eventos) {
                e.Imagen = this.configuration.GetValue<string>("AWS:BucketUrl") + e.Imagen;
            }
            return View(eventos);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Evento evento, IFormFile file) {
            evento.Imagen = file.FileName;
            using (Stream stream = file.OpenReadStream()) {
                await this.serviceS3.UploadFileAsync(evento.Imagen, stream);
            }
            await this.service.Create(evento);
            return RedirectToAction("Index");
        }
    }
}
