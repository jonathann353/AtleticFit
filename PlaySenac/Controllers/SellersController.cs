using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using PlaySenac.Data;
using PlaySenac.Models;
using PlaySenac.Models.ViewModels;
using System.Linq;

namespace PlaySenac.Controllers
{
    public class SellersController : Controller
    {
        private readonly PlaySenacContext _context;

        public SellersController(PlaySenacContext context) { 
            _context = context;
        }

        public IActionResult Index() { 

            //cria uma lista com o salario do func.
            var sellers = _context.Seller.Include("Department").ToList();

            //filtra os func. que ganham meno sque 10k
            var trainee = sellers.Where(s => s.Salary < 10000);

            //lista ordenada pelo nome e salario
            var sallersAscNomeSalary = sellers.OrderBy(s => s.Name).ThenBy(s => s.Salary);

            return View(sallersAscNomeSalary);
        }

        public IActionResult Create() { 
            //Cria uma instância do SellerViewModel
            //Essa instância, vai ter duas propriedades
            //Um vendedor e uma lista de departamentos
            var viewModel = new SellerFormViewModel();
            //carregando os departamentos do banco
            viewModel.Departments = _context.Department.ToList();
            //Encaminha os dados para a view
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(Seller seller) {
            /*Testa se foi passado um objeto vendedor*/
            if (seller == null) { 
                /*Retorna não página não encontrada*/
                return NotFound();
            }
            //seller.Department = _context.Department.FirstOrDefault();
            //seller.DepartmentId = seller.Department.Id;

            /*Adicionar o vendedor no banco de dados*/
            _context.Add(seller);
            /*Confirma/PersistenceMode as adição do vendedor no banco*/
            _context.SaveChanges();
            /*Redireciona para a action index*/
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id) {
            //Busca o vendedor com o ID passado por parâmetro
            var seller = _context.Seller
                .Include("Department")
                .FirstOrDefault(s => s.Id == id);
            //SE não achar um registro com o ID informado,
            //    retorna null
            if (seller == null) {
                return NotFound();
            }

            return View(seller);
        }

        public IActionResult Edit(int id)
        {
            //Verificar se existe um vendedor com o id passado por parâmetro
            Seller seller = _context.Seller.FirstOrDefault(s => s.Id == id);

            if (seller == null)
            {
                return NotFound();
            }

            //Criar uma lista de departamentos
            List<Department> departments = _context.Department.ToList();

            //Cria uma instância do viewmodel
            SellerFormViewModel viewModel = new SellerFormViewModel();
            viewModel.Departments = departments;
            viewModel.Seller = seller;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(Seller seller)
        {
            if (seller == null)
            {
                return NotFound();
            }

            //_context.Seller.Update(seller);
            //Podemos chamar o update sem informar a tabela
            _context.Update(seller);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            //Busca no banco de dados o vendedor com o id informado
            Seller seller = _context.Seller.Include("Department").FirstOrDefault(s => s.Id == id);

            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Seller seller = _context.Seller.FirstOrDefault(s => s.Id == id);

            if (seller == null)
            {
                return NotFound();
            }

            _context.Remove(seller);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Report() {
            var sellers = _context.Seller.Include("Department").ToList();
            
            ViewData["TotalFolhaPagamento"] = sellers.Sum(s => s.Salary);

            ViewData["MaiorSalario"] = sellers.Max(s => s.Salary);

            ViewData["MenorSalario"] = sellers.Min(s => s.Salary);

            ViewData["MediaSalaario"] = sellers.Average(s => s.Salary);

            ViewData["Ricos"] = sellers.Count(s => s.Salary >= 10000);


            return View(sellers);
        
        }
    }
}
