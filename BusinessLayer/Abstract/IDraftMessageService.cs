using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IDraftMessageService
    {
        List<DraftMessage> GetList(string DraftSenderMail);
        List<DraftMessage> GetListReceiver(string DraftReceiverMail);
        List<DraftMessage> GetListSender(string DraftSenderMail);
        void AddDraftMessage(DraftMessage draftMessage);
        DraftMessage GetDraftMessage(int id);
        void DeleteDraftMessage(DraftMessage draftMessage);
        void UpdateDraftMessage(DraftMessage draftMessage);
    }
}
