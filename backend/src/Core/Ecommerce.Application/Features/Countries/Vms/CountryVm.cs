namespace Ecommerce.Application.Features.Countries.Vms;

public class CountryVm
{

    public int Id { get; set; }
    public string? Name { get; set; }


    // CARACTERES QUE REPRESENTAN AL PAIS
    public string? Iso2 { get; set; }
    public string? Iso3 { get; set; }

}
