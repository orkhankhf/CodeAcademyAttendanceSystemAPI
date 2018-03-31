﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CodeAcademyAttendanceSystemAPI.Models;
using CodeAcademyAttendanceSystem.Models.PasswordSecurity;
using System.Text;

namespace CodeAcademyAttendanceSystemAPI.Controllers
{
    public class StudentController : ApiController
    {
        CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();

        ReturnJsonObject ReturnJsonObject = new ReturnJsonObject();

        StudentIPAddress StudentIPAddress = new StudentIPAddress();

        //ApproveAttendance Action'da istifadə edilən property'lər
        DateTime today = Convert.ToDateTime(DateTime.Now.ToShortDateString());

        //CheckStudentLogin Action'da istifadə edilən property'lər
        string JsonString;


        //Tələbə mobil application ilə giriş edəndə
        [HttpGet]
        public HttpResponseMessage Login(string student_email, string student_password, string student_device_id)
        {
            // Aşağıda ReturnJsonObject methoduna parametr kimi ötürmək üçün HttpResponseMessage tipində bir cavab yaradılır...
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //URL'dən gələn email və password dəyərləri boşdursa (və ya null)...
            if (student_email == null || student_email == "" || student_password == null || student_password == "")
            {
                JsonString = "{" +
                                    "\"Success\" : false," +
                                    "\"ErrorMessage\" : \"Email və ya şifrə daxil edilməyib!\"," +
                                    "\"SuccessMessage\" : null," +
                                    "\"SetNewPassword\" : false " +
                                "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }

            //Əgər tələbənin cihazının ID adresi oxunmadısa...
            if (student_device_id == null || student_device_id == "")
            {
                JsonString = "{" +
                                   "\"Success\" : false," +
                                   "\"ErrorMessage\" : \"Cihazın ID adresi oxunmadı!\"," +
                                   "\"SuccessMessage\" : null," +
                                   "\"SetNewPassword\" : false " +
                               "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }

            //Tələbə Code Academy'dən kənarda hesaba daxil olmağa cəhd edirsə...
            if (!StudentIPAddress.Check())
            {
                JsonString = "{" +
                                   "\"Success\" : false," +
                                   "\"ErrorMessage\" : \"Siz Code Academy'nin Wifi'na qoşulmamısınız!\"," +
                                   "\"SuccessMessage\" : null," +
                                   "\"SetNewPassword\" : false " +
                               "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }

            //Databazadan form'dan gələn email'ə uyğun emailı seç
            Students check_logged_student_informations = db.Students.Where(s => s.student_email == student_email).FirstOrDefault();

            //Əgər Form'dan gələn email'ə uyğun nəticə tapılmadısa...
            if (check_logged_student_informations == null)
            {
                JsonString = "{" +
                                   "\"Success\" : false," +
                                   "\"ErrorMessage\" : \"Email düzgün daxil edilməyib!\"," +
                                   "\"SuccessMessage\" : null," +
                                   "\"SetNewPassword\" : false " +
                               "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }

            //Əgər email'in şifrəsi düzgün daxil edilməyibsə...
            if (!PasswordStorage.VerifyPassword(student_password, check_logged_student_informations.student_password))
            {
                JsonString = "{" +
                                   "\"Success\" : false," +
                                   "\"ErrorMessage\" : \"Şifrə düzgün daxil edilməyib!\"," +
                                   "\"SuccessMessage\" : null," +
                                   "\"SetNewPassword\" : false " +
                               "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }

            //Əgər tələbə hesabına ilk dəfə (və ya resetləndikdən sonra ilk dəfə) daxil olursa...
            if (check_logged_student_informations.student_first_login == true)
            {
                JsonString = "{" +
                                   "\"Success\" : true," +
                                   "\"ErrorMessage\" : null," +
                                   "\"SuccessMessage\" : null," +
                                   "\"SetNewPassword\" : true " +
                               "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }

            //Əgər tələbənin giriş etməyə çalışdığı cihaz ilk dəfə giriş etdiyi cihaz deyilsə...
            if (check_logged_student_informations.student_device_id != student_device_id)
            {
                JsonString = "{" +
                                   "\"Success\" : false," +
                                   "\"ErrorMessage\" : \"Xahiş edirik öz cihazınızla daxil olun (Əgər cihazınızı dəyişmisinizsə, zəhmət olmasa müəlliminizə bildirin.)\"," +
                                   "\"SuccessMessage\" : null," +
                                   "\"SetNewPassword\" : false " +
                               "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }

            //Əgər tələbə müvəffəqiyyətlə giriş edibsə...
            if (check_logged_student_informations.student_id > 0)
            {
                JsonString = "{" +
                                   "\"Success\" : true," +
                                   "\"ErrorMessage\" : null," +
                                   "\"SuccessMessage\" : " +
                                                            "{" +
                                                               "\"StudentId\" : " + check_logged_student_informations.student_id + "," +
                                                               "\"StudentName\" : \"" + check_logged_student_informations.student_name + "\"," +
                                                               "\"StudentSurname\" : \"" + check_logged_student_informations.student_surname + "\"," +
                                                               "\"StudentAccountStatus\" : " + check_logged_student_informations.student_status.ToString().ToLower() +
                                                           "}," +
                                   "\"SetNewPassword\" : false " +
                               "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }

            //Əgər yuxarıdakı şərtlərin heç biri ödənmirsə...
            JsonString = "{" +
                                   "\"Success\" : false," +
                                   "\"ErrorMessage\" : \"CA216587: Xəta baş verdi! \"," +
                                   "\"SuccessMessage\" : null," +
                                   "\"SetNewPassword\" : false " +
                               "}";
            return ReturnJsonObject.StringToJson(JsonString, response);
        }

        [HttpGet]
        public HttpResponseMessage SetNewPassword(string student_email, string student_password, string student_new_password, string student_device_id)
        {
            // Aşağıda ReturnJsonObject methoduna parametr kimi ötürmək üçün HttpResponseMessage tipində bir cavab yaradılır...
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //URL'dən gələn email, password və cihazın ID dəyərləri boşdursa (və ya null)...
            if (student_email == null || student_email == "" || student_password == null || student_password == "" || student_device_id == null || student_device_id == "")
            {
                JsonString = "{" +
                                    "\"Success\" : false," +
                                    "\"ErrorMessage\" : \"Xahiş edirik programı bağlayıb təkrar giriş edərək yenidən cəhd edin\"," +
                                    "\"SuccessMessage\" : null," +
                                    "\"SetNewPassword\" : false " +
                                "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }

            //Tələbə Code Academy'dən kənarda şifrəni dəyişməyə cəhd edirsə...
            if (!StudentIPAddress.Check())
            {
                JsonString = "{" +
                                   "\"Success\" : false," +
                                   "\"ErrorMessage\" : \"Siz Code Academy'nin Wifi'na qoşulmamısınız!\"," +
                                   "\"SuccessMessage\" : null," +
                                   "\"SetNewPassword\" : false " +
                               "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }

            //Databazadan form'dan gələn email'ə uyğun emailı seç
            Students select_student_information_for_reset_password = db.Students.Where(s => s.student_email == student_email).FirstOrDefault();

            //Əgər Form'dan gələn email'ə uyğun nəticə tapılmadısa və ya email'in şifrəsi düzgün daxil edilməyibsə...
            if (select_student_information_for_reset_password == null || !PasswordStorage.VerifyPassword(student_password, select_student_information_for_reset_password.student_password))
            {
                JsonString = "{" +
                                   "\"Success\" : false," +
                                   "\"ErrorMessage\" : \"Məlumatlar düzgün deyil! Xahiş edirik programı bağlayıb təkrar giriş edərək yenidən cəhd edin\"," +
                                   "\"SuccessMessage\" : null," +
                                   "\"SetNewPassword\" : false " +
                               "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }

            //Tələbənin şifrəsini update et
            try
            {
                select_student_information_for_reset_password.student_password = PasswordStorage.CreateHash(student_new_password);
                select_student_information_for_reset_password.student_device_id = student_device_id;
                select_student_information_for_reset_password.student_first_login = false;
                db.SaveChanges();
                JsonString = "{" +
                                   "\"Success\" : true," +
                                   "\"ErrorMessage\" : null," +
                                   "\"SuccessMessage\" : \"Şifrəniz yeniləndi. Zəhmət olmasa yeni şifrənizlə hesabınıza daxil olun. \"," +
                                   "\"SetNewPassword\" : false " +
                               "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }
            catch
            {
                //Əgər yuxarıdakı şərtlərin heç biri ödənmirsə...
                JsonString = "{" +
                                   "\"Success\" : false," +
                                   "\"ErrorMessage\" : \"CA127953: Xəta baş verdi! \"," +
                                   "\"SuccessMessage\" : null," +
                                   "\"SetNewPassword\" : false " +
                               "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }
        }

        [HttpGet]
        public HttpResponseMessage ApproveAttendance(string student_email, string student_password, string student_device_id, string qr_code)
        {
            // Aşağıda ReturnJsonObject methoduna parametr kimi ötürmək üçün HttpResponseMessage tipində bir cavab yaradılır...
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //Tələbə Code Academy'dən kənarda QR kodu təsdiqləmək istəyirsə...
            //if (!StudentIPAddress.Check())
            //{
            //    JsonString = "{" +
            //                       "\"Success\" : false," +
            //                       "\"ErrorMessage\" : \"Siz Code Academy'nin Wifi'na qoşulmamısınız!\"," +
            //                       "\"SuccessMessage\" : null," +
            //                       "\"SetNewPassword\" : false " +
            //                   "}";
            //    return ReturnJsonObject.StringToJson(JsonString, response);
            //}

            //URL'dən gələn email, password və ya cihazın ID dəyərləri boşdursa (və ya null)...
            if (student_email == null || student_email == "" || student_password == null || student_password == "" || student_device_id == null || student_device_id == "")
            {
                JsonString = "{" +
                                    "\"Success\" : false," +
                                    "\"ErrorMessage\" : \"Xahiş edirik programı bağlayıb təkrar giriş edərək yenidən cəhd edin\"," +
                                    "\"SuccessMessage\" : null," +
                                    "\"SetNewPassword\" : false " +
                                "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }

            //URL'dən Qr kod dəyəri boşdursa (və ya null)...
            if (qr_code == null || qr_code == "")
            {
                JsonString = "{" +
                                    "\"Success\" : false," +
                                    "\"ErrorMessage\" : \"Qr Kod oxunmadı!\"," +
                                    "\"SuccessMessage\" : null," +
                                    "\"SetNewPassword\" : false " +
                                "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }

            try
            {
                int exiting_user_id = (from s in db.Students
                                   join g in db.Groups on s.student_group_id equals g.group_id
                                   join q in db.Qr_Codes on s.student_group_id equals q.qr_codes_group_id
                                   where s.student_email == student_email && s.student_password == student_password && s.student_device_id == student_device_id
                                   && q.qr_codes_status == true
                                   && q.qr_codes_date == today
                                   && q.qr_codes_value == qr_code
                                   select new
                                   {
                                       s.student_id
                                   }).First().student_id;

                if (exiting_user_id > 0)
                {
                    Students_Attendance student_attendance = db.Students_Attendance.Where(a => a.students_attendance_date == today && a.students_attendance_status == false && a.students_attendance_student_id == exiting_user_id).First();
                    student_attendance.students_attendance_status = true;
                    student_attendance.students_attendance_sender_ip = StudentIPAddress.Get();
                    db.SaveChanges();
                }
                JsonString = "{" +
                                    "\"Success\" : true," +
                                    "\"ErrorMessage\" : \"QR Code oxundu. Attendance update edildi. Dersdesiniz\"," +
                                    "\"SuccessMessage\" : null," +
                                    "\"SetNewPassword\" : false " +
                                "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }
            catch
            {
                JsonString = "{" +
                                    "\"Success\" : false," +
                                    "\"ErrorMessage\" : \"Xahiş edirik programı bağlayıb təkrar giriş edərək yenidən cəhd edin\"," +
                                    "\"SuccessMessage\" : null," +
                                    "\"SetNewPassword\" : false " +
                                "}";
                return ReturnJsonObject.StringToJson(JsonString, response);
            }
        }
    }
}
