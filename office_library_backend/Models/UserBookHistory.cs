//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace office_library_backend.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserBookHistory
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public Nullable<System.DateTime> DateTaken { get; set; }
        public Nullable<System.DateTime> DateReturned { get; set; }
    
        public virtual Book Book { get; set; }
        public virtual MyUser MyUser { get; set; }
    }
}
