using System;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MicroMLVisualizer.Models;
using MicroMLVisualizer.Models.AST;
using MicroMLVisualizer.Models.Parser;
using MicroMLVisualizer.Models.ViewModels;

namespace MicroMLVisualizer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new HomeViewModel());
        }

        [HttpPost]
        public IActionResult Parse(string code)
        {
            var viewModel = new HomeViewModel
            {
                Code = code
            };

            try
            {
                // Parse the code
                var lexer = new Lexer(code);
                var tokens = lexer.Tokenize();
                var parser = new Parser(tokens);
                var ast = parser.Parse();

                // Convert AST to JSON for visualization
                viewModel.ParseResult = JsonSerializer.Serialize(ast.ToJson());
                viewModel.Success = true;
            }
            catch (Exception ex)
            {
                viewModel.ErrorMessage = ex.Message;
                viewModel.Success = false;
            }

            return View("Index", viewModel);
        }
    }
}
