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
    public class MessageManager : IMessageService
    {
        IMessageDal _messageDal;
        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public void AddMessage(Message message)
        {
            _messageDal.Insert(message);
        }

        public void DeleteMessage(Message message)
        {
            _messageDal.Delete(message);
        }

        public List<Message> GetListInbox(string ReceiverMail)
        {
            return _messageDal.List(x => x.ReceiverMail == ReceiverMail && x.Remove == false, x => x.MessageDate);
        }

        public List<Message> GetListInboxNotRead(string ReceiverMail)
        {
            return _messageDal.List(x => x.ReceiverMail == ReceiverMail && x.Remove == false && x.IsRead == false);
        }

        public List<Message> GetListInboxRemoved(string ReceiverMail)
        {
            return _messageDal.List(x => x.ReceiverMail == ReceiverMail && x.Remove == true, x => x.MessageDate);
        }

        public List<Message> GetListReceiver(string ReceiverMail)
        {
            return _messageDal.List(x => x.ReceiverMail == ReceiverMail);
        }

        public List<Message> GetListSendbox(string SenderMail)
        {
            return _messageDal.List(x => x.SenderMail == SenderMail && x.Remove == false, x => x.MessageDate);
        }

        public List<Message> GetListSendboxRemoved(string SenderMail)
        {
            return _messageDal.List(x => x.SenderMail == SenderMail && x.Remove == true, x => x.MessageDate);
        }

        public List<Message> GetListSender(string SenderMail)
        {
            return _messageDal.List(x => x.SenderMail == SenderMail);
        }

        public Message GetMessage(int id)
        {
            return _messageDal.Get(x => x.MessageId == id);
        }

        public void UpdateMessage(Message message)
        {
            _messageDal.Update(message);
        }
    }
}
