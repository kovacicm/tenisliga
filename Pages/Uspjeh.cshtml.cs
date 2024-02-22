using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace tenisLigaOmis.Pages;

public class UspjehModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public UspjehModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }


}


