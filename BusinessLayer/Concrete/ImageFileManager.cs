using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ImageFileManager : IImageFileService
    {
        IImageFileDal _imageFile;
        public ImageFileManager(IImageFileDal imageFile)
        {
            _imageFile = imageFile;
        }

        public void AddImageFile(ImageFile imageFile)
        {
            _imageFile.Insert(imageFile);
        }

        public void DeleteImageFile(ImageFile imageFile)
        {
            _imageFile.Delete(imageFile);
        }

        public ImageFile GetImageFile(int id)
        {
            return _imageFile.Get(x => x.ImageId == id);
        }

        public List<ImageFile> GetList()
        {
            return _imageFile.List();
        }

        public List<ImageFile> GetListActive()
        {
            return _imageFile.List(x => x.ImageRemove == false);
        }

        public void UpdateImageFile(ImageFile imageFile)
        {
            _imageFile.Update(imageFile);
        }
    }
}
