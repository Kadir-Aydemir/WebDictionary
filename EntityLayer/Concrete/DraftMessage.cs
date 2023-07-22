using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class DraftMessage
    {
        [Key]
        public int DraftMessageId { get; set; }

        [StringLength(50)]
        public string DraftSenderMail { get; set; }

        [StringLength(50)]
        public string DraftReceiverMail { get; set; }

        [StringLength(100)]
        public string DraftSubject { get; set; }

        [MaxLength]
        public string DraftMessageContent { get; set; }

        public DateTime DraftMessageDate { get; set; }

    }
}
