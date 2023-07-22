using EntityLayer;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IContentService
    {
        List<Content> GetListAll();
        List<Content> GetList(string parameter);
        List<Content> GetListById(int id);
        List<Content> GetListByIdOrderasc(int id);
        List<Content> GetListByIdOrderLike(int id);
        List<Content> GetListByWriterId(int id);
        List<Content> GetListByWriterIdAll(int id);
        List<Content> GetListByHeadingId(int id);
        void AddContent(Content content);
        Content GetContent(int id);
        Content GetContentInclude(int id);
        void DeleteContent(Content content);
        void UpdateContent(Content content);
        List<HeadingContentChart> ListHeadingContent();
        List<WriterContentChart> ListWriterContent();
    }
}
