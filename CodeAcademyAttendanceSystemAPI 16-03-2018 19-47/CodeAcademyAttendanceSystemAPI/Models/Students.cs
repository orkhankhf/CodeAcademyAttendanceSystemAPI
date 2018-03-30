//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CodeAcademyAttendanceSystemAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Students
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Students()
        {
            this.Students_Attendance = new HashSet<Students_Attendance>();
        }
    
        public int student_id { get; set; }
        public string student_email { get; set; }
        public string student_password { get; set; }
        public string student_name { get; set; }
        public string student_surname { get; set; }
        public string student_father_name { get; set; }
        public string student_phone { get; set; }
        public Nullable<int> student_group_id { get; set; }
        public Nullable<int> student_gender_id { get; set; }
        public string student_device_id { get; set; }
        public Nullable<bool> student_first_login { get; set; }
        public Nullable<bool> student_status { get; set; }
    
        public virtual Genders Genders { get; set; }
        public virtual Groups Groups { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Students_Attendance> Students_Attendance { get; set; }
    }
}
