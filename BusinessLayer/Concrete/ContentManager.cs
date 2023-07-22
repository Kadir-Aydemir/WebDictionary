using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ContentManager : IContentService
    {
        IContentDal _contentDal;
        IHeadingDal _headingDal;
        IWriterDal _writerDal;

        public ContentManager(IContentDal contentDal, IHeadingDal headingDal, IWriterDal writerDal)
        {
            _contentDal = contentDal;
            _headingDal = headingDal;
            _writerDal = writerDal;
        }

        public void AddContent(Content content)
        {
            _contentDal.Insert(content);
        }

        public void DeleteContent(Content content)
        {
            _contentDal.Delete(content);
        }

        public Content GetContent(int id)
        {
            return _contentDal.Get(x => x.ContentID == id);
        }

        public Content GetContentInclude(int id)
        {
            return _contentDal.GetFirstOrDefaultInclude(
                filter: x => x.ContentID == id,
                children: new Expression<Func<Content, object>>[] { x => x.Heading, x => x.Writer }
                );
        }

        public List<Content> GetList(string parameter)
        {
            return _contentDal.List(x => x.ContentCaption.Contains(parameter));
        }

        public List<Content> GetListAll()
        {
            return _contentDal.List();
        }

        public List<Content> GetListByHeadingId(int id)
        {
            return _contentDal.List(x => x.ContentRemove == false && x.HeadingID == id);
        }

        public List<Content> GetListById(int id)
        {
            return _contentDal.List(x => x.HeadingID == id && x.ContentRemove == false, x => x.ContentDate);
        }

        public List<Content> GetListByIdOrderasc(int id)
        {
            return _contentDal.ListOrderAsc(x => x.HeadingID == id && x.ContentRemove == false, x => x.ContentDate);
        }

        public List<Content> GetListByIdOrderLike(int id)
        {
            return _contentDal.ListOrderInt(x => x.HeadingID == id && x.ContentRemove == false, x => x.Likes.Count);
        }

        public List<Content> GetListByWriterId(int id)
        {
            return _contentDal.List(x => x.WriterID == id && x.ContentRemove == false);
        }

        public List<Content> GetListByWriterIdAll(int id)
        {
            return _contentDal.List(x => x.WriterID == id);
        }

        public List<HeadingContentChart> ListHeadingContent()
        {
            var result = _contentDal.ListChart(x => x.HeadingID, x => new HeadingContentChart
            {
                HeadingID = x.Key,
                Count = x.Count()
            });

            var headingIDs = result.Select(x => x.HeadingID).ToList();
            var headingNames = _headingDal.List(c => headingIDs.Contains(c.HeadingID))
                                        .ToDictionary(c => c.HeadingID, c => c.HeadingName);

            foreach (var item in result)
            {
                if (headingNames.ContainsKey(item.HeadingID))
                {
                    item.HeadingName = headingNames[item.HeadingID];
                }
            }
            return result;
        }

        public List<WriterContentChart> ListWriterContent()
        {
            var result = _contentDal.ListChart(x => (int)x.WriterID, x => new WriterContentChart
            {
                WriterID = x.Key,
                Count = x.Count()
            });

            var writerIDs = result.Select(x => x.WriterID).ToList();
            var writerNames = _writerDal.List(c => writerIDs.Contains(c.WriterID))
                                        .ToDictionary(c => c.WriterID, c => c.WriterName + " " + c.WriterSurname);

            foreach (var item in result)
            {
                if (writerNames.ContainsKey(item.WriterID))
                {
                    item.WriterName = writerNames[item.WriterID];
                }
            }
            return result;
        }

        public void UpdateContent(Content content)
        {
            _contentDal.Update(content);
        }
    }
}
