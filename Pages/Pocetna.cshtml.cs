using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace tenisLigaOmis.Pages;

public class PocetnaModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public PocetnaModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }


}