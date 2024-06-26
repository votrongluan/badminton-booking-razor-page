namespace WebAppRazor.Pages
{
    public class IndexModel : AuthorPageServiceModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            LoadAccountFromSession();
        }
    }
}
