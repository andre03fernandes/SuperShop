﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Helpers;
using SuperShop.Models;
using System;
using System.Threading.Tasks;

namespace SuperShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public ProductsController(IProductRepository productRepository, IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper)
        {
            _productRepository = productRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_productRepository.GetAllWithUsers()); // Assim cada entidade tem a possibilidade de ser ordenada de maneira diferente
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                }

                var product = _converterHelper.ToProduct(model, imageId, true);

                product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _productRepository.CreateAsync(product); // Não será preciso gravar aqui pois já gravamos no repositório
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound"); 
            }
            var model = _converterHelper.ToProductViewModel(product);
            return View(model);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                    }

                    var product = _converterHelper.ToProduct(model, imageId, false);

                    product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Products/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // CRIAR UMA VIEW COM UMA MENSAGEM MAIS BONITA
        {
            var product = await _productRepository.GetByIdAsync(id);

            try
            {
                //throw new Exception("Exceção de Teste");
                await _productRepository.DeleteAsync(product);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{product.Name} provavelmente está a ser usado!";
                    ViewBag.ErrorMessage = $"{product.Name} não pode ser apagado visto haverem encomendas que o usam.<br><br>" +
                        $"Experimente primeiro apagar todas as encomendas que estão a usar, " +
                        $"e torne novamente a apagá-lo"; 
                }
                return View("Error");
            }
        }

        public IActionResult ProductsNotFound()
        {
            return View();
        }
    }
}