using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.ImageManagement;
using Microsoft.Extensions.Options;

namespace Ecommerce.Infrastructure.ImageCloudinary;

public class ManageImageService : IManageImageService
{
    //ACCEDER A CloudinarySettings
    public CloudinarySettings _CloudinarySettings { get; }

    public ManageImageService(IOptions<CloudinarySettings> cloudinarySettings)
    {
        _CloudinarySettings = cloudinarySettings.Value;
    }

    // CARGAR LA IMAGEN EN CloudinarySettings
    public async Task<ImageResponse> UploadImage(ImageData imageStream)
    {
        // validar cuenta de CloudinarySettings
        var account = new Account(
            _CloudinarySettings.CloudName,
            _CloudinarySettings.ApiKey,
            _CloudinarySettings.ApiSecret
         );

        var cloudinary = new Cloudinary(account);

        // SUBIR EL ACRCHIVO A UN OBJETO  
        var uploadImage = new ImageUploadParams()
        {
            File = new FileDescription(imageStream.Nombre, imageStream.ImageStream)
        };

        // HACER LA SUBIDA
        var uploadResult = await cloudinary.UploadAsync(uploadImage);
      
        if(uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return new ImageResponse
            {
                PublicId = uploadImage.PublicId,
                Url = uploadResult.Url.ToString()
            };
        }


        throw new Exception("No se pudo guardar la imagen"); 
    }
}
