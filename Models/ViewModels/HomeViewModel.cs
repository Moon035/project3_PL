namespace MicroMLVisualizer.Models.ViewModels
{
    public class HomeViewModel
    {
        public string Code { get; set; } = "fun x -> x + 1";
        public string ParseResult { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
        public bool Success { get; set; } = true;
    }
}
