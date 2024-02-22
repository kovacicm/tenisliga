using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace tenisLigaOmis.Pages;

public class NeuspjehModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public NeuspjehModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }


}


