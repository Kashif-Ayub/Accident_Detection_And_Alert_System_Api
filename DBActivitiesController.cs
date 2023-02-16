using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccidentDetectionApi.Models;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;


namespace AccidentDetectionApi.Controllers
{
    public class DBActivitiesController : ApiController
    {
        Accident_Detection_ProjectEntities23 db = new Accident_Detection_ProjectEntities23();






        //************************************* To Login  ********************************
        [HttpGet]
        public HttpResponseMessage GetAllAlert()
        {
            try
            {


              //  var alerts = (from usr in db.UserDatas join cntct in db.Contacts on usr.uid equals cntct.uid join noti in db.Notifications on cntct.uid equals noti.uid where noti.status == "NotSafe" select new { usr.uname, usr.uid, usr.uphno, noti.nid, noti.forceoncabin, noti.speed, noti.Message, noti.longitude, noti.latitude, noti.hitside, noti.totalforce, noti.car, noti.accidentdate, noti.accidenttime, noti.status }).ToList();
                var alerts = (from usr in db.UserDatas join noti in db.Notifications on usr.uid equals noti.uid where noti.status == "NotSafe"  select new { usr.uname, usr.uid, usr.uphno, noti.nid, noti.forceoncabin, noti.speed, noti.Message, noti.longitude, noti.latitude, noti.hitside, noti.totalforce, noti.car, noti.accidentdate, noti.accidenttime, noti.status }).ToList();

                if (alerts.Count > 0)
                {
                    alerts.Reverse();
                
                    return Request.CreateResponse(HttpStatusCode.OK, alerts);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound, "No Alerts");

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetAllAlert1()
        {
            try
            {
                var alerts = (from usr in db.UserDatas join noti in db.Notifications on usr.uid equals noti.uid select new { usr.uname, usr.uid, usr.uphno, noti.nid, noti.forceoncabin, noti.speed, noti.Message, noti.longitude, noti.latitude, noti.hitside, noti.totalforce, noti.car, noti.accidentdate, noti.accidenttime, noti.status }).ToList();

              //  var alerts = (from usr in db.UserDatas join cntct in db.Notifications on usr.uid equals cntct.uid join noti in db.Notifications on usr.uid equals noti.uid where noti.status=="Safe"||noti.status=="NotSafe"  select new { usr.uname, usr.uid, usr.uphno, noti.nid, noti.forceoncabin, noti.speed, noti.Message, noti.longitude, noti.latitude, noti.hitside, noti.totalforce, noti.car, noti.accidentdate, noti.accidenttime, noti.status }).ToList();
                if (alerts.Count > 0)
                {
                    
                    alerts.Reverse();

                    return Request.CreateResponse(HttpStatusCode.OK, alerts);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound, "No Alerts");

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetHistoryByDate(String date) {
            try {
                var alerts = (from usr in db.UserDatas join noti in db.Notifications on usr.uid equals noti.uid where noti.accidentdate==date select new { usr.uname, usr.uid, usr.uphno, noti.nid, noti.forceoncabin, noti.speed, noti.Message, noti.longitude, noti.latitude, noti.hitside, noti.totalforce, noti.car, noti.accidentdate, noti.accidenttime, noti.status }).ToList();

                //  var alerts = (from usr in db.UserDatas join cntct in db.Notifications on usr.uid equals cntct.uid join noti in db.Notifications on usr.uid equals noti.uid where noti.status=="Safe"||noti.status=="NotSafe"  select new { usr.uname, usr.uid, usr.uphno, noti.nid, noti.forceoncabin, noti.speed, noti.Message, noti.longitude, noti.latitude, noti.hitside, noti.totalforce, noti.car, noti.accidentdate, noti.accidenttime, noti.status }).ToList();
                if (alerts.Count > 0)
                {

                    alerts.Reverse();

                    return Request.CreateResponse(HttpStatusCode.OK, alerts);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound, "No Alerts");

            } catch (Exception e) {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }


        }


        [HttpPost]

        public HttpResponseMessage RegisterPolice(UserData u) {

            try {
                var acc = db.UserDatas.Where(ud=>ud.uphno==u.uphno&&ud.role=="police").FirstOrDefault();
                
                if (acc==null) {
                    u.uname = "POLICE";
                    u.role = "police";
                    db.UserDatas.Add(u);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK,u);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound,"Already have Account");
              
                  

            } catch (Exception e) {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }


        }


        [HttpPost]


        public HttpResponseMessage Login(String phno,String Password) {

            try {

                var log = (from usr in db.UserDatas where usr.uphno==phno && usr.password==Password select new { usr.uname,usr.uid,usr.role}).ToList() ;
                if (log.Count==1) {

                    return Request.CreateResponse(HttpStatusCode.OK,log[0]);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "Invalid Phno or Password");

            } catch (Exception ex) {


                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }




        }


        //************************************* TO Register  ********************************
        private BucklePoint SetCarBuckles(String cartype,int vid) {
            BucklePoint bp = new BucklePoint();
            switch (cartype) {
                case "Honda Civic 2019":
                    bp.bpid = 0;
                    bp.doors =5;
                    bp.roof =15;
                    bp.engine =25;
                    bp.fbumper =5;
                    bp.fleft1 =10;
                    bp.fleft2 =10;
                    bp.fleft3 =15;
                    bp.fleft4 =10;
                    bp.fleft5 =15;
                    bp.fleft6 =10;
                    bp.fleft7 =15;
                    bp.fleft8 =0;
                    bp.fleft9 =0;
                    bp.fleft10 =0;
                    bp.rbumper =25;
                    bp.rleft1 =15;
                    bp.rleft2 =15;
                    bp.rleft3 =0;
                    bp.rleft4 =0;
                    bp.rleft5 =0;
                    bp.rleft6 =0;
                    bp.vid = vid;


                    break;
                case "Suzuki Alto":
                    bp.bpid = 0;
                    bp.doors = 2;
                    bp.roof = 5;
                    bp.engine = 10;
                    bp.fbumper = 5;
                    bp.fleft1 = 5;
                    bp.fleft2 = 7.5;
                    bp.fleft3 = 7.5;
                    bp.fleft4 = 5;
                    bp.fleft5 = 7.5;
                    bp.fleft6 = 7.5;
                    bp.fleft7 = 5;
                    bp.fleft8 = 10;
                    bp.fleft9 = 10;
                    bp.fleft10 = 5;
                    bp.rbumper = 5;
                    bp.rleft1 = 10;
                    bp.rleft2 = 10;
                    bp.rleft3 = 0;
                    bp.rleft4 = 0;
                    bp.rleft5 = 0;
                    bp.rleft6 = 0;
                    bp.vid = vid;
                    break;
                case "Toyota Corolla 2014":
                    bp.bpid = 0;
                    bp.doors = 4;
                    bp.roof = 12;
                    bp.engine = 20;
                    bp.fbumper = 4;
                    bp.fleft1 = 10;
                    bp.fleft2 = 10;
                    bp.fleft3 = 6;
                    bp.fleft4 = 15;
                    bp.fleft5 = 20;
                    bp.fleft6 = 15;
                    bp.fleft7 = 6;
                    bp.fleft8 = 0;
                    bp.fleft9 = 0;
                    bp.fleft10 = 0;
                    bp.rbumper = 4;
                    bp.rleft1 = 10;
                    bp.rleft2 = 10;
                    bp.rleft3 = 10;
                    bp.rleft4 = 10;
                    bp.rleft5 = 6;
                    bp.rleft6 = 6;
                    bp.vid = vid;
                    break;
                case "Vitz":
                    bp.bpid = 0;
                    bp.doors = 2;
                    bp.roof = 7;
                    bp.engine = 12;
                    bp.fbumper = 3;
                    bp.fleft1 = 12;
                    bp.fleft2 = 12;
                    bp.fleft3 = 3;
                    bp.fleft4 = 12;
                    bp.fleft5 = 12;
                    bp.fleft6 = 3;
                    bp.fleft7 = 0;
                    bp.fleft8 = 0;
                    bp.fleft9 = 0;
                    bp.fleft10 = 0;
                    bp.rbumper = 3;
                    bp.rleft1 = 3;
                    bp.rleft2 = 12;
                    bp.rleft3 = 12;
                    bp.rleft4 = 3;
                    bp.rleft5 = 0;
                    bp.rleft6 = 0;
                    bp.vid = vid;
                    break;

            }


            return bp;
        }
        private void SetCarBucklesDistance(String cartype,int bpid) {
            

            switch (cartype)
            {
                case "Honda Civic 2019":
                    BucklePointsDistance bpd1 = new BucklePointsDistance();
                    bpd1.bpdid =0;
                    bpd1.bpid = bpid;
                    bpd1.dfrom = "fleft1";
                    bpd1.dfleft1 =0;
                    bpd1.dfleft2 =500;
                    bpd1.dfleft3 =150;
                    bpd1.dfleft4 =700;
                    bpd1.dfleft5 =400;
                    bpd1.dfleft6 =300;
                    bpd1.dfleft7 =350;
                    bpd1.dfleft8 =400;
                    bpd1.dfleft9 =550;
                    bpd1.dfleft10 =0;
                    bpd1.drleft1 =3900;
                    bpd1.drleft2 =4300;
                    bpd1.drleft3 =4500;
                    bpd1.drleft4 =4550;
                    bpd1.drleft5 =0;
                    bpd1.drleft6 =0;
                    db.BucklePointsDistances.Add(bpd1);
                    db.SaveChanges();
                    BucklePointsDistance bpd2 = new BucklePointsDistance();
                    bpd2.bpdid = 0;
                    bpd2.bpid = bpid;
                    bpd2.dfrom = "fleft2";
                    bpd2.dfleft1 = 500;
                    bpd2.dfleft2 = 0;
                    bpd2.dfleft3 = 300;
                    bpd2.dfleft4 = 150;
                    bpd2.dfleft5 = 550;
                    bpd2.dfleft6 = 510;
                    bpd2.dfleft7 = 500;
                    bpd2.dfleft8 = 400;
                    bpd2.dfleft9 = 420;
                    bpd2.dfleft10 = 0;
                    bpd2.drleft1 = 4300;
                    bpd2.drleft2 = 3900;
                    bpd2.drleft3 = 4550;
                    bpd2.drleft4 = 4500;
                    bpd2.drleft5 = 0;
                    bpd2.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd2);
                    db.SaveChanges();
                    BucklePointsDistance bpd3 = new BucklePointsDistance();
                    bpd3.bpdid = 0;
                    bpd3.bpid = bpid;
                    bpd3.dfrom = "fleft3";
                    bpd3.dfleft1 = 150;
                    bpd3.dfleft2 = 300;
                    bpd3.dfleft3 = 0;
                    bpd3.dfleft4 = 350;
                    bpd3.dfleft5 = 220;
                    bpd3.dfleft6 = 180;
                    bpd3.dfleft7 = 230;
                    bpd3.dfleft8 = 300;
                    bpd3.dfleft9 = 420;
                    bpd3.dfleft10 = 0;
                    bpd3.drleft1 = 4100;
                    bpd3.drleft2 = 4300;
                    bpd3.drleft3 = 4200;
                    bpd3.drleft4 = 4300;
                    bpd3.drleft5 = 0;
                    bpd3.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd3);
                    db.SaveChanges();
                    BucklePointsDistance bpd4 = new BucklePointsDistance();
                    bpd4.bpdid = 0;
                    bpd4.bpid = bpid;
                    bpd4.dfrom = "fleft4";
                    bpd4.dfleft1 = 700;
                    bpd4.dfleft2 = 150;
                    bpd4.dfleft3 = 350;
                    bpd4.dfleft4 = 0;
                    bpd4.dfleft5 = 320;
                    bpd4.dfleft6 = 300;
                    bpd4.dfleft7 = 280;
                    bpd4.dfleft8 = 200;
                    bpd4.dfleft9 = 240;
                    bpd4.dfleft10 = 0;
                    bpd4.drleft1 = 4000;
                    bpd4.drleft2 = 3900;
                    bpd4.drleft3 = 4150;
                    bpd4.drleft4 = 4100;
                    bpd4.drleft5 = 0;
                    bpd4.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd4);
                    db.SaveChanges();
                    BucklePointsDistance bpd5 = new BucklePointsDistance();
                    bpd5.bpdid = 0;
                    bpd5.bpid = bpid;
                    bpd5.dfrom = "fleft5";
                    bpd5.dfleft1 = 400;
                    bpd5.dfleft2 = 550;
                    bpd5.dfleft3 = 220;
                    bpd5.dfleft4 = 320;
                    bpd5.dfleft5 = 0;
                    bpd5.dfleft6 = 100;
                    bpd5.dfleft7 = 200;
                    bpd5.dfleft8 = 250;
                    bpd5.dfleft9 = 500;
                    bpd5.dfleft10 = 0;
                    bpd5.drleft1 = 3800;
                    bpd5.drleft2 = 3900;
                    bpd5.drleft3 = 4000;
                    bpd5.drleft4 = 4100;
                    bpd5.drleft5 = 0;
                    bpd5.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd5);
                    db.SaveChanges();
                    BucklePointsDistance bpd6 = new BucklePointsDistance();
                    bpd6.bpdid = 0;
                    bpd6.bpid = bpid;
                    bpd6.dfrom = "fleft6";
                    bpd6.dfleft1 = 400;
                    bpd6.dfleft2 = 550;
                    bpd6.dfleft3 = 220;
                    bpd6.dfleft4 = 320;
                    bpd6.dfleft5 = 0;
                    bpd6.dfleft6 = 100;
                    bpd6.dfleft7 = 200;
                    bpd6.dfleft8 = 250;
                    bpd6.dfleft9 = 500;
                    bpd6.dfleft10 = 0;
                    bpd6.drleft1 = 3800;
                    bpd6.drleft2 = 3900;
                    bpd6.drleft3 = 4000;
                    bpd6.drleft4 = 4100;
                    bpd6.drleft5 = 0;
                    bpd6.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd6);
                    db.SaveChanges();
                    break;
                case "Suzuki Alto":
                    BucklePointsDistance bpd7 = new BucklePointsDistance();
                    bpd7.bpdid = 0;
                    bpd7.bpid = bpid;
                    bpd7.dfrom = "fleft1";
                    bpd7.dfleft1 = 0;
                    bpd7.dfleft2 = 500;
                    bpd7.dfleft3 = 150;
                    bpd7.dfleft4 = 700;
                    bpd7.dfleft5 = 400;
                    bpd7.dfleft6 = 300;
                    bpd7.dfleft7 = 350;
                    bpd7.dfleft8 = 400;
                    bpd7.dfleft9 = 550;
                    bpd7.dfleft10 = 0;
                    bpd7.drleft1 = 3900;
                    bpd7.drleft2 = 4300;
                    bpd7.drleft3 = 4500;
                    bpd7.drleft4 = 4550;
                    bpd7.drleft5 = 0;
                    bpd7.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd7);
                    db.SaveChanges();
                    BucklePointsDistance bpd8 = new BucklePointsDistance();
                    bpd8.bpdid = 0;
                    bpd8.bpid = bpid;
                    bpd8.dfrom = "fleft2";
                    bpd8.dfleft1 = 500;
                    bpd8.dfleft2 = 0;
                    bpd8.dfleft3 = 300;
                    bpd8.dfleft4 = 150;
                    bpd8.dfleft5 = 550;
                    bpd8.dfleft6 = 510;
                    bpd8.dfleft7 = 500;
                    bpd8.dfleft8 = 400;
                    bpd8.dfleft9 = 420;
                    bpd8.dfleft10 = 0;
                    bpd8.drleft1 = 4300;
                    bpd8.drleft2 = 3900;
                    bpd8.drleft3 = 4550;
                    bpd8.drleft4 = 4500;
                    bpd8.drleft5 = 0;
                    bpd8.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd8);
                    db.SaveChanges();
                    BucklePointsDistance bpd9 = new BucklePointsDistance();
                    bpd9.bpdid = 0;
                    bpd9.bpid = bpid;
                    bpd9.dfrom = "fleft3";
                    bpd9.dfleft1 = 150;
                    bpd9.dfleft2 = 300;
                    bpd9.dfleft3 = 0;
                    bpd9.dfleft4 = 350;
                    bpd9.dfleft5 = 220;
                    bpd9.dfleft6 = 180;
                    bpd9.dfleft7 = 230;
                    bpd9.dfleft8 = 300;
                    bpd9.dfleft9 = 420;
                    bpd9.dfleft10 = 0;
                    bpd9.drleft1 = 4100;
                    bpd9.drleft2 = 4300;
                    bpd9.drleft3 = 4200;
                    bpd9.drleft4 = 4300;
                    bpd9.drleft5 = 0;
                    bpd9.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd9);
                    db.SaveChanges();
                    BucklePointsDistance bpd10 = new BucklePointsDistance();
                    bpd10.bpdid = 0;
                    bpd10.bpid = bpid;
                    bpd10.dfrom = "fleft4";
                    bpd10.dfleft1 = 700;
                    bpd10.dfleft2 = 150;
                    bpd10.dfleft3 = 350;
                    bpd10.dfleft4 = 0;
                    bpd10.dfleft5 = 320;
                    bpd10.dfleft6 = 300;
                    bpd10.dfleft7 = 280;
                    bpd10.dfleft8 = 200;
                    bpd10.dfleft9 = 240;
                    bpd10.dfleft10 = 0;
                    bpd10.drleft1 = 4000;
                    bpd10.drleft2 = 3900;
                    bpd10.drleft3 = 4150;
                    bpd10.drleft4 = 4100;
                    bpd10.drleft5 = 0;
                    bpd10.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd10);
                    db.SaveChanges();
                    BucklePointsDistance bpd11 = new BucklePointsDistance();
                    bpd11.bpdid = 0;
                    bpd11.bpid = bpid;
                    bpd11.dfrom = "fleft5";
                    bpd11.dfleft1 = 400;
                    bpd11.dfleft2 = 550;
                    bpd11.dfleft3 = 220;
                    bpd11.dfleft4 = 320;
                    bpd11.dfleft5 = 0;
                    bpd11.dfleft6 = 100;
                    bpd11.dfleft7 = 200;
                    bpd11.dfleft8 = 250;
                    bpd11.dfleft9 = 500;
                    bpd11.dfleft10 = 0;
                    bpd11.drleft1 = 3800;
                    bpd11.drleft2 = 3900;
                    bpd11.drleft3 = 4000;
                    bpd11.drleft4 = 4100;
                    bpd11.drleft5 = 0;
                    bpd11.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd11);
                    db.SaveChanges();
                    BucklePointsDistance bpd12 = new BucklePointsDistance();
                    bpd12.bpdid = 0;
                    bpd12.bpid = bpid;
                    bpd12.dfrom = "fleft6";
                    bpd12.dfleft1 = 400;
                    bpd12.dfleft2 = 550;
                    bpd12.dfleft3 = 220;
                    bpd12.dfleft4 = 320;
                    bpd12.dfleft5 = 0;
                    bpd12.dfleft6 = 100;
                    bpd12.dfleft7 = 200;
                    bpd12.dfleft8 = 250;
                    bpd12.dfleft9 = 500;
                    bpd12.dfleft10 = 0;
                    bpd12.drleft1 = 3800;
                    bpd12.drleft2 = 3900;
                    bpd12.drleft3 = 4000;
                    bpd12.drleft4 = 4100;
                    bpd12.drleft5 = 0;
                    bpd12.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd12);
                    db.SaveChanges();
                    break;
                case "Toyota Corolla 2014":
                    BucklePointsDistance bpd13 = new BucklePointsDistance();
                    bpd13.bpdid = 0;
                    bpd13.bpid = bpid;
                    bpd13.dfrom = "fleft1";
                    bpd13.dfleft1 = 0;
                    bpd13.dfleft2 = 500;
                    bpd13.dfleft3 = 150;
                    bpd13.dfleft4 = 700;
                    bpd13.dfleft5 = 400;
                    bpd13.dfleft6 = 300;
                    bpd13.dfleft7 = 350;
                    bpd13.dfleft8 = 400;
                    bpd13.dfleft9 = 550;
                    bpd13.dfleft10 = 0;
                    bpd13.drleft1 = 3900;
                    bpd13.drleft2 = 4300;
                    bpd13.drleft3 = 4500;
                    bpd13.drleft4 = 4550;
                    bpd13.drleft5 = 0;
                    bpd13.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd13);
                    db.SaveChanges();
                    BucklePointsDistance bpd14 = new BucklePointsDistance();
                    bpd14.bpdid = 0;
                    bpd14.bpid = bpid;
                    bpd14.dfrom = "fleft2";
                    bpd14.dfleft1 = 500;
                    bpd14.dfleft2 = 0;
                    bpd14.dfleft3 = 300;
                    bpd14.dfleft4 = 150;
                    bpd14.dfleft5 = 550;
                    bpd14.dfleft6 = 510;
                    bpd14.dfleft7 = 500;
                    bpd14.dfleft8 = 400;
                    bpd14.dfleft9 = 420;
                    bpd14.dfleft10 = 0;
                    bpd14.drleft1 = 4300;
                    bpd14.drleft2 = 3900;
                    bpd14.drleft3 = 4550;
                    bpd14.drleft4 = 4500;
                    bpd14.drleft5 = 0;
                    bpd14.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd14);
                    db.SaveChanges();
                    BucklePointsDistance bpd15 = new BucklePointsDistance();
                    bpd15.bpdid = 0;
                    bpd15.bpid = bpid;
                    bpd15.dfrom = "fleft3";
                    bpd15.dfleft1 = 150;
                    bpd15.dfleft2 = 300;
                    bpd15.dfleft3 = 0;
                    bpd15.dfleft4 = 350;
                    bpd15.dfleft5 = 220;
                    bpd15.dfleft6 = 180;
                    bpd15.dfleft7 = 230;
                    bpd15.dfleft8 = 300;
                    bpd15.dfleft9 = 420;
                    bpd15.dfleft10 = 0;
                    bpd15.drleft1 = 4100;
                    bpd15.drleft2 = 4300;
                    bpd15.drleft3 = 4200;
                    bpd15.drleft4 = 4300;
                    bpd15.drleft5 = 0;
                    bpd15.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd15);
                    db.SaveChanges();
                    BucklePointsDistance bpd16 = new BucklePointsDistance();
                    bpd16.bpdid = 0;
                    bpd16.bpid = bpid;
                    bpd16.dfrom = "fleft4";
                    bpd16.dfleft1 = 700;
                    bpd16.dfleft2 = 150;
                    bpd16.dfleft3 = 350;
                    bpd16.dfleft4 = 0;
                    bpd16.dfleft5 = 320;
                    bpd16.dfleft6 = 300;
                    bpd16.dfleft7 = 280;
                    bpd16.dfleft8 = 200;
                    bpd16.dfleft9 = 240;
                    bpd16.dfleft10 = 0;
                    bpd16.drleft1 = 4000;
                    bpd16.drleft2 = 3900;
                    bpd16.drleft3 = 4150;
                    bpd16.drleft4 = 4100;
                    bpd16.drleft5 = 0;
                    bpd16.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd16);
                    db.SaveChanges();
                    BucklePointsDistance bpd17 = new BucklePointsDistance();
                    bpd17.bpdid = 0;
                    bpd17.bpid = bpid;
                    bpd17.dfrom = "fleft5";
                    bpd17.dfleft1 = 400;
                    bpd17.dfleft2 = 550;
                    bpd17.dfleft3 = 220;
                    bpd17.dfleft4 = 320;
                    bpd17.dfleft5 = 0;
                    bpd17.dfleft6 = 100;
                    bpd17.dfleft7 = 200;
                    bpd17.dfleft8 = 250;
                    bpd17.dfleft9 = 500;
                    bpd17.dfleft10 = 0;
                    bpd17.drleft1 = 3800;
                    bpd17.drleft2 = 3900;
                    bpd17.drleft3 = 4000;
                    bpd17.drleft4 = 4100;
                    bpd17.drleft5 = 0;
                    bpd17.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd17);
                    db.SaveChanges();
                    BucklePointsDistance bpd18 = new BucklePointsDistance();
                    bpd18.bpdid = 0;
                    bpd18.bpid = bpid;
                    bpd18.dfrom = "fleft6";
                    bpd18.dfleft1 = 400;
                    bpd18.dfleft2 = 550;
                    bpd18.dfleft3 = 220;
                    bpd18.dfleft4 = 320;
                    bpd18.dfleft5 = 0;
                    bpd18.dfleft6 = 100;
                    bpd18.dfleft7 = 200;
                    bpd18.dfleft8 = 250;
                    bpd18.dfleft9 = 500;
                    bpd18.dfleft10 = 0;
                    bpd18.drleft1 = 3800;
                    bpd18.drleft2 = 3900;
                    bpd18.drleft3 = 4000;
                    bpd18.drleft4 = 4100;
                    bpd18.drleft5 = 0;
                    bpd18.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd18);
                    db.SaveChanges();
                    break;
                case "Vitz":

                    BucklePointsDistance bpd20 = new BucklePointsDistance();
                    bpd20.bpdid = 0;
                    bpd20.bpid = bpid;
                    bpd20.dfrom = "fleft1";
                    bpd20.dfleft1 = 0;
                    bpd20.dfleft2 = 500;
                    bpd20.dfleft3 = 150;
                    bpd20.dfleft4 = 700;
                    bpd20.dfleft5 = 400;
                    bpd20.dfleft6 = 300;
                    bpd20.dfleft7 = 350;
                    bpd20.dfleft8 = 400;
                    bpd20.dfleft9 = 550;
                    bpd20.dfleft10 = 0;
                    bpd20.drleft1 = 3900;
                    bpd20.drleft2 = 4300;
                    bpd20.drleft3 = 4500;
                    bpd20.drleft4 = 4550;
                    bpd20.drleft5 = 0;
                    bpd20.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd20);
                    db.SaveChanges();
                    BucklePointsDistance bpd21 = new BucklePointsDistance();
                    bpd21.bpdid = 0;
                    bpd21.bpid = bpid;
                    bpd21.dfrom = "fleft2";
                    bpd21.dfleft1 = 500;
                    bpd21.dfleft2 = 0;
                    bpd21.dfleft3 = 300;
                    bpd21.dfleft4 = 150;
                    bpd21.dfleft5 = 550;
                    bpd21.dfleft6 = 510;
                    bpd21.dfleft7 = 500;
                    bpd21.dfleft8 = 400;
                    bpd21.dfleft9 = 420;
                    bpd21.dfleft10 = 0;
                    bpd21.drleft1 = 4300;
                    bpd21.drleft2 = 3900;
                    bpd21.drleft3 = 4550;
                    bpd21.drleft4 = 4500;
                    bpd21.drleft5 = 0;
                    bpd21.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd21);
                    db.SaveChanges();
                    BucklePointsDistance bpd22 = new BucklePointsDistance();
                    bpd22.bpdid = 0;
                    bpd22.bpid = bpid;
                    bpd22.dfrom = "fleft3";
                    bpd22.dfleft1 = 150;
                    bpd22.dfleft2 = 300;
                    bpd22.dfleft3 = 0;
                    bpd22.dfleft4 = 350;
                    bpd22.dfleft5 = 220;
                    bpd22.dfleft6 = 180;
                    bpd22.dfleft7 = 230;
                    bpd22.dfleft8 = 300;
                    bpd22.dfleft9 = 420;
                    bpd22.dfleft10 = 0;
                    bpd22.drleft1 = 4100;
                    bpd22.drleft2 = 4300;
                    bpd22.drleft3 = 4200;
                    bpd22.drleft4 = 4300;
                    bpd22.drleft5 = 0;
                    bpd22.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd22);
                    db.SaveChanges();
                    BucklePointsDistance bpd23 = new BucklePointsDistance();
                    bpd23.bpdid = 0;
                    bpd23.bpid = bpid;
                    bpd23.dfrom = "fleft4";
                    bpd23.dfleft1 = 700;
                    bpd23.dfleft2 = 150;
                    bpd23.dfleft3 = 350;
                    bpd23.dfleft4 = 0;
                    bpd23.dfleft5 = 320;
                    bpd23.dfleft6 = 300;
                    bpd23.dfleft7 = 280;
                    bpd23.dfleft8 = 200;
                    bpd23.dfleft9 = 240;
                    bpd23.dfleft10 = 0;
                    bpd23.drleft1 = 4000;
                    bpd23.drleft2 = 3900;
                    bpd23.drleft3 = 4150;
                    bpd23.drleft4 = 4100;
                    bpd23.drleft5 = 0;
                    bpd23.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd23);
                    db.SaveChanges();
                    BucklePointsDistance bpd24 = new BucklePointsDistance();
                    bpd24.bpdid = 0;
                    bpd24.bpid = bpid;
                    bpd24.dfrom = "fleft5";
                    bpd24.dfleft1 = 400;
                    bpd24.dfleft2 = 550;
                    bpd24.dfleft3 = 220;
                    bpd24.dfleft4 = 320;
                    bpd24.dfleft5 = 0;
                    bpd24.dfleft6 = 100;
                    bpd24.dfleft7 = 200;
                    bpd24.dfleft8 = 250;
                    bpd24.dfleft9 = 500;
                    bpd24.dfleft10 = 0;
                    bpd24.drleft1 = 3800;
                    bpd24.drleft2 = 3900;
                    bpd24.drleft3 = 4000;
                    bpd24.drleft4 = 4100;
                    bpd24.drleft5 = 0;
                    bpd24.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd24);
                    db.SaveChanges();
                    BucklePointsDistance bpd25 = new BucklePointsDistance();
                    bpd25.bpdid = 0;
                    bpd25.bpid = bpid;
                    bpd25.dfrom = "fleft6";
                    bpd25.dfleft1 = 400;
                    bpd25.dfleft2 = 550;
                    bpd25.dfleft3 = 220;
                    bpd25.dfleft4 = 320;
                    bpd25.dfleft5 = 0;
                    bpd25.dfleft6 = 100;
                    bpd25.dfleft7 = 200;
                    bpd25.dfleft8 = 250;
                    bpd25.dfleft9 = 500;
                    bpd25.dfleft10 = 0;
                    bpd25.drleft1 = 3800;
                    bpd25.drleft2 = 3900;
                    bpd25.drleft3 = 4000;
                    bpd25.drleft4 = 4100;
                    bpd25.drleft5 = 0;
                    bpd25.drleft6 = 0;
                    db.BucklePointsDistances.Add(bpd25);
                    db.SaveChanges();
                    break;

            }

          
        }

        [HttpPost]

        public HttpResponseMessage Register(User_Owner_Vehicle uov)
        {

            try
            {

                var usralready = (from usr in db.UserDatas where usr.uphno==uov.phno select new { usr.uname} ).ToList();

                if (!(usralready.Count>0))
                {
                    UserData ud = new UserData();
                    ud.uid = 0;
                    ud.uname = uov.uname;
                    ud.uphno = uov.phno;
                    ud.password = uov.password;
                    ud.role = "user";
                    db.UserDatas.Add(ud);
                    Vehicle vh = new Vehicle();
                    vh.vid = 0;
                    vh.vnumber = uov.vnumber;
                    vh.vtype = uov.vtype;
                    db.Vehicles.Add(vh);
                    db.SaveChanges();
                    var uid = (from usr in db.UserDatas where usr.uphno == uov.phno && usr.password == uov.password select new { usr.uid }).ToList();
                    var vid = (from veh in db.Vehicles where veh.vnumber == uov.vnumber && veh.vtype == uov.vtype select new { veh.vid }).ToList();
                    OwnerVehicle ov = new OwnerVehicle();
                    ov.uid = uid[0].uid;
                    ov.vid = vid[0].vid;
                    db.OwnerVehicles.Add(ov);
                    db.SaveChanges();

                    BucklePoint bp = SetCarBuckles(uov.vtype, vid[0].vid);
                    db.BucklePoints.Add(bp);
                    db.SaveChanges();
                    var bpid = (from b in db.BucklePoints where b.vid == bp.vid select new { b.bpid }).ToList();
                    SetCarBucklesDistance(uov.vtype, bpid[0].bpid);

                    return Request.CreateResponse(HttpStatusCode.OK, "Inserted Successfully");
                }
                else {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,"Already have an account related to this number");
                }


            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //************************************* TO Get User    ********************************
        [HttpPost]
        public HttpResponseMessage GetCars(String phno) {

            try {

                //from o in db.OwnerVehicles
        //                             join u in db.UserDatas on o.uid_phno equals u.phno
        //                             join v in db.Vehicles on o.vid equals v.vid where o.uid_phno==uid
                var ucars =( from usr in db.UserDatas join ov in db.OwnerVehicles on usr.uid equals ov.uid join veh in db.Vehicles on ov.vid equals veh.vid where usr.uphno==phno  select new {veh.vtype }).ToList();
              
                


                return Request.CreateResponse(HttpStatusCode.OK,ucars);

            } catch (Exception ex) {

                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }


        }

        [HttpGet]
        public HttpResponseMessage GetCarBuckles(String car,String uphno) {

            try {
                var buckles = (from u in db.UserDatas join ov in db.OwnerVehicles on u.uid equals ov.uid join veh in db.Vehicles on ov.vid equals veh.vid join b in db.BucklePoints on veh.vid equals b.vid where veh.vtype==car && u.uphno==uphno select b   ).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, buckles);
            } catch (Exception ex ) {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
        }


        [HttpPost]
        public HttpResponseMessage UpdateWireFrame(BucklePoint bp) {

            try {
                bool modifiedversion = false;
                var vid = bp.vid;
                var veh = db.OwnerVehicles.Where(o=>o.vid==vid).FirstOrDefault();
                var vtype = db.Vehicles.Find(vid);
                if (vtype.vtype== "Honda Civic 2019Modified" || vtype.vtype== "Suzuki AltoModified" || vtype.vtype== "Toyota Corolla 2014Modified" || vtype.vtype== "VitzModified") { modifiedversion = true; }
                var vtp = (from o in db.OwnerVehicles join v in db.Vehicles on o.vid equals v.vid where o.uid==veh.uid select v.vtype   ).ToList();
                
                if (vtp.Contains(vtype.vtype+"Modified")||modifiedversion==true)
                {
                    if (modifiedversion == false)
                    {
                        var upvid = (from o in db.OwnerVehicles join v in db.Vehicles on o.vid equals v.vid where o.uid == veh.uid && v.vtype == vtype.vtype + "Modified" select v.vid).ToList();
                        bp.vid = upvid[0];

                        var prevbp = db.BucklePoints.Where(bv => bv.vid == bp.vid).FirstOrDefault();
                        bp.bpid = prevbp.bpid;
                        db.Entry(prevbp).CurrentValues.SetValues(bp);

                        db.SaveChanges();
                    }
                    else {
                        var upvid = (from o in db.OwnerVehicles join v in db.Vehicles on o.vid equals v.vid where o.uid == veh.uid && v.vtype == vtype.vtype select v.vid).ToList();
                        bp.vid = upvid[0];

                        var prevbp = db.BucklePoints.Where(bv => bv.vid == bp.vid).FirstOrDefault();
                        bp.bpid = prevbp.bpid;
                        db.Entry(prevbp).CurrentValues.SetValues(bp);

                        db.SaveChanges();
                    }

                }
                else {

                    Vehicle v = new Vehicle();
                    v.vtype = vtype.vtype + "Modified";
                    v.vnumber = vtype.vnumber;
                    v.vid = 0;
                    db.Vehicles.Add(v);
                    db.SaveChanges();
                    var vd = db.Vehicles.Where(ve=>ve.vnumber==vtype.vnumber&&ve.vtype== vtype.vtype + "Modified").FirstOrDefault();
                    OwnerVehicle ovh = new OwnerVehicle();
                    ovh.uid = veh.uid;
                    ovh.vid = vd.vid;
                    db.OwnerVehicles.Add(ovh);
                    db.SaveChanges();
                    bp.vid = vd.vid;
                    db.BucklePoints.Add(bp);
                    db.SaveChanges();
                }

               



                return Request.CreateResponse(HttpStatusCode.OK, "WireFrame Modified");


            }
            catch (Exception ex) {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
                    
                    
                    }
        }
        //**************************   To SAVE Contacts Added By the User 
        [HttpPost]
        public HttpResponseMessage AddContact(Contact c)
        {
            try
            {


                //var usrid = (from usr in db.UserDatas where usr.uphno == c.uphno select new { usr.uid }).ToList();


                Contact econtact = new Contact();

                Contact svcont = new Contact();
                String cont1 = c.cphno;
                bool chk = false;



                Regex rgx = new Regex(@"[\d]{4}\s[\d]{7}");
                Regex rgx1 = new Regex(@"[+]{1}[\d]2\s[\d]{3}\s[\d]{7}");
                Match m2 = rgx.Match(cont1);
                Match m1 = rgx1.Match(cont1);
                if (m1.Success)
                {

                    chk = true;
                    var v = cont1.Split(' ').ToList();
                    cont1 = "";
                    v.RemoveAt(0);
                    v.Remove(" ");
                    v.Insert(0, "0");
                    foreach (String cv in v)
                    {

                        cont1 += cv;
                    }
                    econtact.cphno = cont1;
                    econtact.cid = c.cid;
                    econtact.uid = c.uid;
                    econtact.cname = c.cname;


                }
                else if (m2.Success)
                {
                    chk = true;
                    var x = cont1.Split(' ').ToList();
                    cont1 = "";
                    x.Remove(" ");
                    foreach (String cv in x)
                    {

                        cont1 += cv;
                    }


                    econtact.cphno = cont1;
                    econtact.cid = c.cid;
                    econtact.uid = c.uid;
                    econtact.cname = c.cname;


                }

                if (chk)
                {

                    svcont = econtact;
                }
                else
                {

                    svcont.uid = c.uid;
                    svcont.cphno = c.cphno;
                    svcont.cid = c.cid;
                    svcont.cname = c.cname;
                }





                var cntct = (from cont in db.Contacts where cont.cphno == svcont.cphno && cont.uid == svcont.uid select new { cont.uid }).ToList();
                if (cntct.Count == 0)
                {
                    db.Contacts.Add(svcont);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Contact Saved Successfully");
                }

                return Request.CreateResponse(HttpStatusCode.Ambiguous, "Contact Already  Saved");


            }
            catch (Exception e)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }



        }

        //**************************   To Get Contacts Added By the User 
        [HttpGet]
        public HttpResponseMessage GetContacts(String uid_phno)
        {
            try
            {
                var cntcs = (from contacts in db.Contacts join usr in db.UserDatas on contacts.uid equals usr.uid where usr.uphno == uid_phno select new { contacts.cid, contacts.uid, contacts.cphno, contacts.cname }).ToList();


                if (cntcs.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, cntcs);
                }

                return Request.CreateResponse(HttpStatusCode.NoContent, "no contacts Added yet");
            }
            catch (Exception e)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }



        }



        //**************************   To Generate a Notification Against an Accident
        [HttpPost]
        public HttpResponseMessage Generate_Notification(Notification noti)
        {

            try
            {
                //var n = db.Notifications.Find(noti.uid_phno);

                //if (n != null)
                //{
                //    db.Entry(n).CurrentValues.SetValues(noti);
                //    db.SaveChanges();
                //    return Request.CreateResponse(HttpStatusCode.OK, "Notification Saved Successfully");
                //}

                //db.Notifications.Add(noti);
                //db.SaveChanges();

             db.Notifications.Add(noti);
                db.SaveChanges();
                var nidlst = (from n in db.Notifications where n.uid==noti.uid select n.nid).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, nidlst[nidlst.Count-1]);
            }
            catch (Exception ex) { return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message); }




        }



        // **************************   To Get Accident Alerts Related to the User
        [HttpPost]
        public HttpResponseMessage MarkAsSafe(int nid) {
            try {
                Notification newnoti =new  Notification();
                var pv = db.Notifications.Find(nid);
                if (pv!=null) {

                    newnoti.nid = pv.nid;
                    newnoti.uid = pv.uid;
                    newnoti.forceoncabin = pv.forceoncabin;
                    newnoti.speed = pv.uid;
                    newnoti.longitude = pv.longitude;
                    newnoti.latitude = pv.latitude;
                    newnoti.Message = pv.Message;
                    newnoti.totalforce = pv.totalforce;
                    newnoti.hitside = pv.hitside;
                    newnoti.car = pv.car;
                    newnoti.accidentdate = pv.accidentdate;
                    newnoti.accidenttime = pv.accidenttime;
                    newnoti.status = "Safe";
                    db.Entry(pv).CurrentValues.SetValues(newnoti);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK,nid);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, nid);
            } catch (Exception ex) {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }

        }

               [HttpGet]
                public HttpResponseMessage GetAlerts(String contact,String date)
        {
            try
            {
                

                var alerts = (from usr in db.UserDatas join cntct in db.Contacts on usr.uid equals cntct.uid join noti in db.Notifications on cntct.uid equals noti.uid where cntct.cphno==contact && noti.accidentdate==date&&noti.status=="NotSafe" select new {usr.uname,usr.uid,usr.uphno,noti.nid,noti.forceoncabin,noti.speed,noti.Message,noti.longitude,noti.latitude,noti.hitside,noti.totalforce,noti.car,noti.accidentdate,noti.accidenttime,noti.status }).ToList();
                if (alerts.Count>0) {

                    return Request.CreateResponse(HttpStatusCode.OK, alerts);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound, "No Alerts");
               
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        public HttpResponseMessage History(String contact)
        {
            try
            {


                var alerts = (from usr in db.UserDatas join cntct in db.Contacts on usr.uid equals cntct.uid join noti in db.Notifications on cntct.uid equals noti.uid where cntct.cphno == contact&&noti.status=="Safe" select new { usr.uname, usr.uid, usr.uphno, noti.nid, noti.forceoncabin, noti.speed, noti.Message, noti.longitude, noti.latitude, noti.hitside, noti.totalforce, noti.car,noti.accidentdate,noti.accidenttime,noti.status }).ToList();
                if (alerts.Count > 0)
                {

                    return Request.CreateResponse(HttpStatusCode.OK, alerts);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound, "No Alerts");

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        //**************************   For Continous Saving Coordinates of Accident  And Tracking Live Location of Accident 

        [HttpPost]
        public HttpResponseMessage UpdateLiveCoordinatesAccident(AccidentLocationTrack trc)
        {
            try
            {
                var tcc1 = (from track in db.AccidentLocationTracks where track.longitude == trc.longitude && track.latitude == trc.latitude && track.uid == trc.uid select new { track.uid}).ToList();
                if (tcc1.Count>0) {
                    return Request.CreateResponse(HttpStatusCode.OK, "Accident Location Update Added  Successfully");
                }
                else {
                    db.AccidentLocationTracks.Add(trc);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Accident Location Update Added  Successfully");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }


        //**************************   For Continous Saving Coordinates of Responder  And Tracking Live Location  of Responder
        [HttpPost]
        public HttpResponseMessage UpdateLiveCoordinatesResponder(ResponderLocationTrack responder)
        {

            try
            {

                //var tcc1 = (from track in db.AccidentLocationTracks where track.longitude == responder.longitude && track.latitude == responder.latitude && track.uid == responder.uid select new { track.uid }).ToList();
                //if (tcc1.Count > 0)
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK, "Accident Location Update Added  Successfully");
                //}
                //else
                //{
                    db.ResponderLocationTracks.Add(responder);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Accident Location Update Added  Successfully");
              //  }
                //Subha isy check krna hai



                //var cid = (from cont in db.Contacts where cont.cphno == responder.cphno && cont.uid == responder.uid select new {
                //    cont.cid

                //}).ToList();
                ////  var usr = db.UserDatas.Where(u => u.phno == id).ToList();
                //var c = cid[0].cid;
                //var resptrack = db.ResponderLocationTracks.Where(trc=> trc.cid==c&&trc.nid==responder.nid).ToList();


                //if (resptrack.Count > 0)
                //{
                //    var rltid = resptrack[0].rltid;
                //    ResponderLocationTrack respondertrack = new ResponderLocationTrack();
                //    respondertrack.rltid = rltid;
                //    respondertrack.cid = c;
                //    respondertrack.latitude = responder.latitude;
                //    respondertrack.longitude = responder.longitude;
                //    respondertrack.nid = responder.nid;
                //    var ptrack = db.ResponderLocationTracks.Find(resptrack[0].rltid);

                //    db.Entry(ptrack).CurrentValues.SetValues(respondertrack);
                //    db.SaveChanges();
                //    return Request.CreateResponse(HttpStatusCode.OK, "Responder Location Updated Successfully");
                //}
                //else {
                //    ResponderLocationTrack respondertrack1 = new ResponderLocationTrack();
                //    respondertrack1.rltid = 0;
                //    respondertrack1.cid = c;
                //    respondertrack1.latitude = responder.latitude;
                //    respondertrack1.longitude = responder.longitude;
                //    respondertrack1.nid = responder.nid;
                //    db.ResponderLocationTracks.Add(respondertrack1);
                //    db.SaveChanges();
                //    return Request.CreateResponse(HttpStatusCode.OK, "Responder Location Added Successfully");
                //}





















                //var cid = (from cont in db.Contacts join resptrc in db.ResponderLocationTracks on cont.cid equals resptrc.cid where cont.uid == responder.uid && cont.cphno == responder.cphno select new {cont.cid }).ToList();


                //ResponderLocationTrack res = new ResponderLocationTrack();

                //res.rltid = 0;
                //res.cid = cid[0].cid;
                //res.latitude = responder.latitude;
                //res.longitude = responder.longitude;

                //return Request.CreateResponse(HttpStatusCode.OK, res);
                //var resp =(from trc in db.ResponderLocationTracks where trc.cid==res.cid select new { trc.rltid,trc.cid,trc.longitude,trc.latitude}).ToList() ;
                //ResponderLocationTrack res1 = db.ResponderLocationTracks.Find(cid[0].cid);
                //if (resp.Count>0)
                //{

                //    db.Entry(res1).CurrentValues.SetValues(res);
                //    db.SaveChanges();
                //    return Request.CreateResponse(HttpStatusCode.OK, "Responder Location Updated Successfully");
                //}

                //db.ResponderLocationTracks.Add(res);
                //db.SaveChanges();
                //return Request.CreateResponse(HttpStatusCode.OK, "Responder Location Added Successfully");


            }
            catch (Exception ex) { return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message); }

        }



        //**************************   To Get the Live Locations Coordinates of Responders


        [HttpGet]
        public HttpResponseMessage GetCoordinatesofResonder(String victim_phone ,int nid)
        {
            try
            {

                var clist = ( from usr in db.UserDatas join  cont in db.Contacts on usr.uid equals cont.uid where usr.uphno==victim_phone select new { cont.cid }).ToList();

              var vuid =( from u in db.UserDatas where u.uphno == victim_phone select u.uid).ToList();
                int vuid1 = vuid[0];
                if (clist.Count > 0)
                {
            
                    List<Track> responders = new List<Track>();
                    Track trc1 ;
                    foreach (var contact in clist)
                    {
                       
                        var cont = db.ResponderLocationTracks.Where(c=>c.cid== vuid1 && c.nid==nid).ToList();
                    
                        if (cont.Count>0)
                        {
                            foreach (var rsp in cont) {
                                trc1 = new Track();
                                var cntct = rsp.cid;
                                var resp = (from c in db.Contacts where c.uid == cntct select new { c.cname, c.cphno }).ToList();
                                if (resp.Count>0) {
                                   // foreach (var rs in resp) {

                                        trc1.cphno = resp[0].cphno;
                                        trc1.cname = resp[0].cname;
                                        trc1.latitude = rsp.latitude;
                                        trc1.longitude = rsp.longitude;
                                        trc1.nid = nid;

                                        responders.Add(trc1); }
                                //}
                            }

                        }

                    }

                    return Request.CreateResponse(HttpStatusCode.OK, responders);

                }

                return Request.CreateResponse(HttpStatusCode.NotFound, "No Contacts Are Added By Victim");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }


        }


      //  **************************   To Get the Live Locations Coordinates of Accident

        [HttpGet]
        public HttpResponseMessage GetCoordinatesofAccident(String victim_phone ,int nid)
        {
            try
            {
                var accidentcoordinates = (from usr in db.UserDatas join acctrack in db.AccidentLocationTracks on usr.uid equals acctrack.uid where usr.uphno==victim_phone &&  acctrack.nid==nid select new {
                    acctrack.altid,
                    acctrack.uid,
                    acctrack.latitude,
                    acctrack.longitude
                    ,acctrack.nid
                }).ToList();
                if (accidentcoordinates.Count > 0)
                {

                    return Request.CreateResponse(HttpStatusCode.OK, accidentcoordinates);
                }
                return Request.CreateResponse(HttpStatusCode.NoContent, "No Coordinates Added");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }


        }



        //Getting Buckles Api Functions
        [HttpGet]


        public HttpResponseMessage GetBuckles(String cartype,String hitside,String uphno) {
            try {
               
                List<Bucklesinfo> buckles = new List<Bucklesinfo>();
                Bucklesinfo buckinfo ;
               

                    var carbuckles= (from u in db.UserDatas join o in db.OwnerVehicles on u.uid equals o.uid join v in db.Vehicles on o.vid equals v.vid join buck in db.BucklePoints on v.vid equals buck.vid where v.vtype==cartype && u.uphno==uphno select new {
                        buck.doors,
                        buck.roof,
                        buck.engine,
                        buck.fbumper,
                        buck.fleft1,
                        buck.fleft2,
                        buck.fleft3,
                        buck.fleft4,
                        buck.fleft5,
                        buck.fleft6,
                        buck.fleft7,
                        buck.fleft8,
                        buck.fleft9,
                        buck.fleft10,
                        buck.rbumper,
                        buck.rleft1,
                        buck.rleft2,
                        buck.rleft3,
                        buck.rleft4,
                        buck.rleft5,
                        buck.rleft6,

                    } ).ToList();
                if (!(carbuckles.Count>0)) {
                    var vid = (from cars in db.Vehicles
                               where cars.vtype == cartype
                               select new
                               {
                                   cars.vid
                               }).ToList();
                    int vd = vid[0].vid;
                    (from buck in db.BucklePoints
                     where buck.vid == vd
                     select new
                     {
                         buck.doors,
                         buck.roof,
                         buck.engine,
                         buck.fbumper,
                         buck.fleft1,
                         buck.fleft2,
                         buck.fleft3,
                         buck.fleft4,
                         buck.fleft5,
                         buck.fleft6,
                         buck.fleft7,
                         buck.fleft8,
                         buck.fleft9,
                         buck.fleft10,
                         buck.rbumper,
                         buck.rleft1,
                         buck.rleft2,
                         buck.rleft3,
                         buck.rleft4,
                         buck.rleft5,
                         buck.rleft6,

                     }).ToList();
                }
                if (carbuckles.Count > 0)
                {
                    switch (cartype)
                    {
                        case "Honda Civic 2019":
                            switch (hitside) {

                                case "Front":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value / 2;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);

                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);


                                    break;
                                case "Back":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                   
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                               

                                    break;
                                case "FrontLeft":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint ="fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value / 2;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);

                                  

                                    break;

                                case "FrontRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value / 2;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);


                                   



                                    break;
                                case "LeftFront":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                

                                    break;
                                case "RightFront":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                             
                                    break;
                                case "door1":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door1";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);



                                    break;
                                case "door2":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door2";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);



                                    break;
                                case "door3":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door3";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);




                                    break;
                                case "door4":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door4";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);




                                    break;

                                case "BackLeft":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "BackRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "LeftBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "RightBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "FrontLeftDiagonal":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);



                                    break;
                                case "FrontRightDiagonal":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "BackLeftDiagonal":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "BackRightDiagonal":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    break;
                            }
                            

                            break;
                        case "Honda Civic 2019Modified":
                            switch (hitside)
                            {

                                case "Front":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value / 2;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);



                                    break;
                                case "FrontLeft":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value / 2;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);



                                    break;

                                case "FrontRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value / 2;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);






                                    break;
                                case "LeftFront":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);


                                    break;
                                case "RightFront":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "door1":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door1";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);



                                    break;
                                case "door2":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door2";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);



                                    break;
                                case "door3":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door3";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);




                                    break;
                                case "door4":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door4";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);




                                    break;

                                case "BackLeft":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "BackRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "LeftBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "RightBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "FrontLeftDiagonal":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);



                                    break;
                                case "FrontRightDiagonal":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "BackLeftDiagonal":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "BackRightDiagonal":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    break;
                            }


                            break;

                        case "Suzuki Alto":
                            switch (hitside)
                            {
                                case "Front":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                  
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft8";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft8.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "Back":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    break;


                                case "FrontLeft":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft8";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft8.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    break;

                                case "FrontRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft9";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft9.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft10";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft10.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "LeftFront":

                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft8";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft8.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft9";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft9.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft10";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft10.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "RightFront":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft10";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft10.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft9";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft9.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft8";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft8.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "door1":

                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door1";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft10";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft10.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft9";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft9.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "door2":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door2";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft8";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft8.Value;
                                    buckles.Add(buckinfo);


                                    break;
                                case "door3":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door3";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);





                                    break;
                                case "door4":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door4";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);

                                    break;

                                case "BackLeft":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "BackRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "LeftBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "RightBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "FrontLeftDiagonal":
                                    break;
                                case "FrontRightDiagonal":
                                    break;
                                case "BackLeftDiagonal":
                                    break;
                                case "BackRightDiagonal":
                                    break;
                            }


                            break;
                        case "Suzuki AltoModified":
                            switch (hitside)
                            {
                                case "Front":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                  
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft8";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft8.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft9";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft9.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft10";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft10.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "Break":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                  
                                    break;


                                case "FrontLeft":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft8";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft8.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    break;

                                case "FrontRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft9";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft9.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft10";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft10.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "LeftFront":

                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft8";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft8.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft9";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft9.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft10";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft10.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "RightFront":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft10";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft10.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft9";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft9.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft8";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft8.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "door1":

                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door1";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft10";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft10.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft9";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft9.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "door2":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door2";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft8";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft8.Value;
                                    buckles.Add(buckinfo);


                                    break;
                                case "door3":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door3";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);





                                    break;
                                case "door4":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door4";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);

                                    break;

                                case "BackLeft":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "BackRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "LeftBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "RightBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "FrontLeftDiagonal":
                                    break;
                                case "FrontRightDiagonal":
                                    break;
                                case "BackLeftDiagonal":
                                    break;
                                case "BackRightDiagonal":
                                    break;
                            }


                            break;
                        case "Toyota Corolla 2014":
                            switch (hitside)
                            {
                                case "Front":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);

                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                   
                                    break;
                                case "Back":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                   
                                    break;

                                case "FrontLeft":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    break;

                                case "FrontRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "LeftFront":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "RightFront":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "door1":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door1";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);




                                    break;
                                case "door2":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door2";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);




                                    break;
                                case "door3":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door3";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft6.Value;
                                    buckles.Add(buckinfo);






                                    break;
                                case "door4":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door4";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft5.Value;
                                    buckles.Add(buckinfo);

                                    break;

                                case "BackLeft":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft5.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "BackRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft6.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "LeftBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft6.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "RightBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft5.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "FrontLeftDiagonal":
                                    break;
                                case "FrontRightDiagonal":
                                    break;
                                case "BackLeftDiagonal":
                                    break;
                                case "BackRightDiagonal":
                                    break;
                            }


                            break;
                        case "Toyota Corolla 2014Modified":
                            switch (hitside)
                            {
                                case "FrontLeft":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    break;

                                case "FrontRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "LeftFront":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "RightFront":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "door1":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door1";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft7";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft7.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);




                                    break;
                                case "door2":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door2";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);




                                    break;
                                case "door3":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door3";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft6.Value;
                                    buckles.Add(buckinfo);






                                    break;
                                case "door4":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door4";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft5.Value;
                                    buckles.Add(buckinfo);

                                    break;

                                case "BackLeft":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft5.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "BackRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft6.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "LeftBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft6.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "RightBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft5.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "FrontLeftDiagonal":
                                    break;
                                case "FrontRightDiagonal":
                                    break;
                                case "BackLeftDiagonal":
                                    break;
                                case "BackRightDiagonal":
                                    break;
                            }


                            break;

                        case "Vitz":
                            switch (hitside)
                            {
                                case "FrontLeft":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo); buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    
                                    break;

                                case "FrontRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo); buckinfo = new Bucklesinfo();
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "LeftFront":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo); 
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "RightFront":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "door1":

                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door1";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "door2":

                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door2";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);



                                    break;
                                case "door3":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door3";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft4.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "door4":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door4";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    break;

                                case "BackLeft":
                                    ///
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "BackRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft4.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "LeftBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft4.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "RightBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "FrontLeftDiagonal":
                                    break;
                                case "FrontRightDiagonal":
                                    break;
                                case "BackLeftDiagonal":
                                    break;
                                case "BackRightDiagonal":
                                    break;
                            }

                            break;
                        case "VitzModified":
                            switch (hitside)
                            {
                                case "FrontLeft":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo); buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);

                                    break;

                                case "FrontRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].fbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo); buckinfo = new Bucklesinfo();
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "LeftFront":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "RightFront":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "engine";
                                    buckinfo.bucklepointabs = carbuckles[0].engine.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "door1":

                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door1";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft6";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft6.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft5";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft5.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "door2":

                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door2";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "fleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].fleft4.Value;
                                    buckles.Add(buckinfo);



                                    break;
                                case "door3":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door3";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft4.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "door4":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "door4";
                                    buckinfo.bucklepointabs = carbuckles[0].doors.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    break;

                                case "BackLeft":
                                    ///
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "BackRight":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rbumper";
                                    buckinfo.bucklepointabs = carbuckles[0].rbumper.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft4.Value;
                                    buckles.Add(buckinfo);

                                    break;
                                case "LeftBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft4.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "RightBack":
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft4";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft4.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft3";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft3.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft2";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft2.Value;
                                    buckles.Add(buckinfo);
                                    buckinfo = new Bucklesinfo();
                                    buckinfo.bucklepoint = "rleft1";
                                    buckinfo.bucklepointabs = carbuckles[0].rleft1.Value;
                                    buckles.Add(buckinfo);
                                    break;
                                case "FrontLeftDiagonal":
                                    break;
                                case "FrontRightDiagonal":
                                    break;
                                case "BackLeftDiagonal":
                                    break;
                                case "BackRightDiagonal":
                                    break;
                            }

                            break;

                    }


                    return Request.CreateResponse(HttpStatusCode.OK,buckles);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound,"No buckle info found related to requested car type");
            } catch (Exception ex) {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }




        }


        ///////////////FINAL TASK IMPORTANT APIS///////////////////////////////


        [HttpGet]

        public HttpResponseMessage MyAccidentHistory(int uid) {

            try {


                var myacchistory = (from acc in db.Notifications where acc.uid==uid select acc).ToList();
                if (myacchistory.Count>0) {

                    return Request.CreateResponse(HttpStatusCode.OK,myacchistory);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "No History");
            } catch (Exception e) {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,e.Message);
            }

        }


[HttpGet]
        public HttpResponseMessage MaxAccidents(String contact)
        {

            try
            {

                var alerts = (from usr in db.UserDatas join cntct in db.Contacts on usr.uid equals cntct.uid join noti in db.Notifications on cntct.uid equals noti.uid where cntct.cphno == contact && noti.status == "Safe" select new { usr.uname, usr.uid, usr.uphno, noti.nid, noti.forceoncabin, noti.speed, noti.Message, noti.longitude, noti.latitude, noti.hitside, noti.totalforce, noti.car, noti.accidentdate, noti.accidenttime, noti.status }).ToList();
                if (alerts.Count > 0)
                {
                   
                   int maxid= alerts.Max(u => u.uid);
                    var maxaccperson = (from usr in db.UserDatas join cntct in db.Contacts on usr.uid equals cntct.uid join noti in db.Notifications on cntct.uid equals noti.uid where cntct.cphno == contact && noti.status == "Safe" && usr.uid==maxid select new { usr.uname, usr.uid, usr.uphno, noti.nid, noti.forceoncabin, noti.speed, noti.Message, noti.longitude, noti.latitude, noti.hitside, noti.totalforce, noti.car, noti.accidentdate, noti.accidenttime, noti.status }).FirstOrDefault();
                    //var maxaccperson = db.UserDatas.Find(maxid);
                    return Request.CreateResponse(HttpStatusCode.OK, maxaccperson);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound, "No Alerts"); ;
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }

        }


        [HttpGet]
        public HttpResponseMessage HistoryVictimyearly(String contact)
        {
            try
            {
                //using (var context = new Accident_Detection_ProjectEntities22())
                //{

                //    var data = context.Notifications
                //        .Where(x => DateTime.Parse(x.accidentdate) >= oneYearAgo)
                //        .ToList();

                //    return Request.CreateResponse(HttpStatusCode.OK, data);
                //}
                var dt = DateTime.Now.AddYears(-1);

                var now = DateTime.Now;
                //DateTime onemonthAgo = DateTime.Parse(dt.ToString(), new System.Globalization.CultureInfo("pt-BR"));
                //DateTime nowdt = DateTime.Parse(now.ToString(), new System.Globalization.CultureInfo("pt-BR"));
                List<dynamic> yearly = new List<dynamic>();
                var alerts = (from usr in db.UserDatas join cntct in db.Contacts on usr.uid equals cntct.uid join noti in db.Notifications on cntct.uid equals noti.uid where cntct.cphno == contact && noti.status == "Safe" select new { usr.uname, usr.uid, usr.uphno, noti.nid, noti.forceoncabin, noti.speed, noti.Message, noti.longitude, noti.latitude, noti.hitside, noti.totalforce, noti.car, noti.accidentdate, noti.accidenttime, noti.status }).ToList();
                if (alerts.Count > 0)
                {
                    foreach (var x in alerts)
                    {
                        //{
                        //    var acd = DateTime.ParseExact(x.accidentdate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        DateTime date = DateTime.Parse(x.accidentdate.ToString(), new System.Globalization.CultureInfo("pt-BR"));
                        //if (DateTime.Parse(x.accidentdate) >= oneYearAgo) {

                        //    yearly.Add(x);
                        //}
                        if (date >= dt && date <= now)
                        {

                            yearly.Add(x);
                        }
                    }
                    if (yearly.Count > 0)
                    {


                        return Request.CreateResponse(HttpStatusCode.OK, alerts);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "No Alerts on Yearly Basis");
                    }
                }

                return Request.CreateResponse(HttpStatusCode.NotFound, "No Alerts");

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpGet]
        public HttpResponseMessage HistoryVictimmonthly(String contact)
        {
            try
            {
                //using (var context = new Accident_Detection_ProjectEntities22())
                //{

                //    var data = context.Notifications
                //        .Where(x => DateTime.Parse(x.accidentdate) >= oneYearAgo)
                //        .ToList();

                //    return Request.CreateResponse(HttpStatusCode.OK, data);
                //}
                var dt = DateTime.Now.AddMonths(-1);

                var now = DateTime.Now;
                //DateTime onemonthAgo = DateTime.Parse(dt.ToString(), new System.Globalization.CultureInfo("pt-BR"));
                //DateTime nowdt = DateTime.Parse(now.ToString(), new System.Globalization.CultureInfo("pt-BR"));
                List<dynamic> yearly = new List<dynamic>();
                var alerts = (from usr in db.UserDatas join cntct in db.Contacts on usr.uid equals cntct.uid join noti in db.Notifications on cntct.uid equals noti.uid where cntct.cphno == contact && noti.status == "Safe" select new { usr.uname, usr.uid, usr.uphno, noti.nid, noti.forceoncabin, noti.speed, noti.Message, noti.longitude, noti.latitude, noti.hitside, noti.totalforce, noti.car, noti.accidentdate, noti.accidenttime, noti.status }).ToList();
                if (alerts.Count > 0)
                {
                    foreach (var x in alerts) { 
                    //{
                    //    var acd = DateTime.ParseExact(x.accidentdate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        DateTime date = DateTime.Parse(x.accidentdate.ToString(), new System.Globalization.CultureInfo("pt-BR"));
                        //if (DateTime.Parse(x.accidentdate) >= oneYearAgo) {

                        //    yearly.Add(x);
                        //}
                        if (date >= dt && date <= now)
                        {

                            yearly.Add(x);
                        }
                    }
                    if (yearly.Count > 0)
                    {


                        return Request.CreateResponse(HttpStatusCode.OK, alerts);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "No Alerts on Monthly Basis");
                    }
                }

                return Request.CreateResponse(HttpStatusCode.NotFound, "No Alerts");

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        //Csv File Creator

        [HttpPost]
        public HttpResponseMessage CreateCsv(Angle ang) {
            try {
                using (StreamWriter sw = File.AppendText(@"F:/Dataset.txt"))
                {
                    sw.WriteLine(("angle:" + ang.angle.ToString() + ",x:" +ang.x.ToString() + ",y:" + ang.y.ToString() + ",z:" + ang.z.ToString())+ "," );
                }
                //using (System.IO.StreamWriter file =
                //     new System.IO.StreamWriter(@"F:/angle.txt"))
                //{
                 
                //        file.WriteLine((ang.x.ToString() + ang.y.ToString()+ang.z.ToString()));
                    
                //}

                return Request.CreateResponse(HttpStatusCode.OK,"Data Inserted in csv");
            } catch (Exception ex) {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
        }





        

        //////**************************   To Get All Available Car Types  
        ////        [HttpGet]

        ////        public HttpResponseMessage Get_Available_Car_Types() {
        ////            try {

        ////                var cars = (from car in db.BucklePoints select new { car.vehicle_type }).ToList();


        ////                if (cars.Count>0) {


        ////                    return Request.CreateResponse(HttpStatusCode.OK,cars);
        ////                }

        ////                return Request.CreateResponse(HttpStatusCode.NotFound,"No Available Cars");


        ////            } catch (Exception ex) {
        ////                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
        ////            }

        ////        }


        ////        //**************************   To Get All Available Car Types And user Car
        ////        [HttpPost]
        ////        public HttpResponseMessage GetCars(String phno) {
        ////            try {
        ////                List<String> ucars = new List<string>();
        ////                List<String> acars = new List<string>();


        ////                var usr =( from owner in db.OwnerVehicles where owner.uid_phno==phno select new {owner.vid}).ToList();

        ////                for (int i=0;i<usr.Count;i++) {

        ////                    var vehicle = db.Vehicles.Find(usr[i].vid);
        ////                    ucars.Add(vehicle.vtype);

        ////                }
        ////                int ucount = ucars.Count;

        ////                var cars= (from car in db.BucklePoints select new { car.vehicle_type }).ToList();
        ////                foreach (var c in cars) {
        ////                    acars.Add(c.vehicle_type);

        ////                }

        ////                foreach (var crs in ucars) {

        ////                    acars.Remove(crs);
        ////                }
        ////                ucars.AddRange(acars);
        ////                ucars.Insert(0,ucount.ToString());
        ////                if (ucars.Count>1) {

        ////                    return Request.CreateResponse(HttpStatusCode.OK, ucars);
        ////                }

        ////                return Request.CreateResponse(HttpStatusCode.NotFound,"No  cars related to this Contact Found");


        ////            } catch(Exception e) {

        ////                return Request.CreateResponse(HttpStatusCode.InternalServerError,e.Message);
        ////            }


        ////        }


































        //************************************* DATA FETCHING FUNCTIONS ********************************

        //        [HttpGet]
        //        public HttpResponseMessage GETALLUSERS() {
        //            try {

        //                var usrs = db.UserDatas.OrderBy(b => b.phno).ToList();

        //                if (usrs.Count > 0)
        //                {

        //                    return Request.CreateResponse(HttpStatusCode.OK, usrs);

        //                }
        //                return Request.CreateResponse(HttpStatusCode.OK, usrs);

        //            } catch (Exception ex) {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }
        //        }

        //        [HttpGet]

        //        public HttpResponseMessage GETUSERBYID(String id) {

        //            try {
        //                var usr = db.UserDatas.Where(u => u.phno == id).ToList();

        //                if (usr.Count > 0) {

        //                    return Request.CreateResponse(HttpStatusCode.OK, usr);

        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No Record Matched With id=" + id);



        //            } catch (Exception ex) {
        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }
        //        }


        //        [HttpGet]


        //        public HttpResponseMessage GetContacts() {

        //            try {
        //                var cntcts =db.Contacts.OrderBy(u=>u.uid_phno).ToList();
        //                if (cntcts.Count>0) {



        //                    return Request.CreateResponse(HttpStatusCode.OK,cntcts);
        //                }



        //                return Request.CreateResponse(HttpStatusCode.OK,"NO RECORD");

        //            } catch (Exception ex) {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
        //            }
        //        }


        //        [HttpGet]

        //        public HttpResponseMessage GETCONTACTSBYID(String uid) {
        //            try {
        //                var cntcs =db.Contacts.Where(c=>c.uid_phno==uid).ToList();
        //                if (cntcs.Count>0) {

        //                    return Request.CreateResponse(HttpStatusCode.OK,cntcs);
        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK,"No Contacts found Related to this user id ="+uid) ;
        //            } catch (Exception ex) {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message);
        //            }


        //        }
        //        [HttpGet]

        //     public   HttpResponseMessage GetNotifications() {


        //            try {

        //                var notification = (from noti in db.Notifications select new {


        //                    noti.uid_phno,
        //                    noti.uname,noti.speed,noti.phno,noti.longitude,noti.latitude,noti.Message

        //                }).ToList();

        //                if (notification.Count>0) {

        //                    return Request.CreateResponse(HttpStatusCode.OK,notification);


        //                }

        //                return  Request.CreateResponse(HttpStatusCode.OK,"No Notifications Found") ;

        //            } catch (Exception ex) {
        //                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message); 
        //            }


        //        }

        //        [HttpGet]

        //        public HttpResponseMessage GetNotificationsByuid(String uid)
        //        {


        //            try
        //            {

        //                var notification = (from noti in db.Notifications where noti.uid_phno==uid
        //                                    select new
        //                                    {


        //                                        noti.uid_phno,
        //                                        noti.uname,
        //                                        noti.speed,
        //                                        noti.phno,
        //                                        noti.longitude,
        //                                        noti.latitude,
        //                                        noti.Message

        //                                    }).ToList();

        //                if (notification.Count > 0)
        //                {

        //                    return Request.CreateResponse(HttpStatusCode.OK, notification);


        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No Notifications Found");

        //            }
        //            catch (Exception ex)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }


        //        }

        //        [HttpGet]
        //        public HttpResponseMessage GetVehicleInfo()
        //        {

        //            try
        //            {

        //                var owner = (from o in db.OwnerVehicles join u in db.UserDatas on o.uid_phno equals u.phno join v in db.Vehicles on o.vid equals v.vid select new {
        //                    v.vid,v.vtype,u.uname,u.phno




        //                }).ToList();
        //                if (owner.Count>0) {

        //                    return Request.CreateResponse(HttpStatusCode.OK,owner);

        //                }
        //                return Request.CreateResponse(HttpStatusCode.OK," no record ");
        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }


        //        }



        //        [HttpGet]
        //        public HttpResponseMessage GetVehicleInfoById(String uid)
        //        {

        //            try
        //            {

        //                var owner = (from o in db.OwnerVehicles
        //                             join u in db.UserDatas on o.uid_phno equals u.phno
        //                             join v in db.Vehicles on o.vid equals v.vid where o.uid_phno==uid
        //                             select new
        //                             {
        //                                 v.vid,
        //                                 v.vtype,
        //                                 u.uname,
        //                                 u.phno




        //                             }).ToList();
        //                if (owner.Count>0)
        //                {

        //                    return Request.CreateResponse(HttpStatusCode.OK, owner);

        //                }
        //                return Request.CreateResponse(HttpStatusCode.OK, " no record ");
        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }


        //        }




        //        [HttpGet]
        //        public HttpResponseMessage GetbucklePoints()
        //        {

        //            try
        //            {

        //                //var v = db.BucklePoints.Find(bid);
        //                var buckles = (from v in db.Vehicles
        //                               join b in db.BucklePoints on v.vid equals b.vid
        //                               select new
        //                               {

        //                                   v.vid,
        //                                   v.vtype,
        //                                  b.Doors,
        //                                 b.Roof,b.F_Left_1,b.F_Left_2,
        //                                   b.F_Left_3,
        //                                   b.F_Left_4,
        //                                   b.F_Left_5,
        //                                   b.F_Left_6,
        //                                   b.F_Left_7,
        //                                   b.F_Left_8,
        //                                   b.F_Left_9,
        //                                   b.F_Left_10,b.R_Left_1,
        //                                   b.R_Left_2,
        //                                   b.R_Left_3,
        //                                   b.R_Left_4,
        //                                   b.R_Left_5,
        //                                   b.R_Left_6,




        //                               }).ToList();
        //                if (buckles.Count > 0)
        //                {

        //                    return Request.CreateResponse(HttpStatusCode.OK, buckles);

        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No buckles found");

        //            }
        //            catch (Exception e)
        //            {


        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
        //            }

        //        }

        //        [HttpGet]
        //        public HttpResponseMessage GetbucklePointsByvid(String vid)
        //        {

        //            try
        //            {

        //                //var v = db.BucklePoints.Find(bid);
        //                var buckles = (from v in db.Vehicles
        //                               join b in db.BucklePoints on v.vid equals b.vid where v.vid==vid
        //                               select new
        //                               {

        //                                   v.vid,
        //                                   v.vtype,
        //                                   b.Doors,
        //                                   b.Roof,
        //                                   b.F_Left_1,
        //                                   b.F_Left_2,
        //                                   b.F_Left_3,
        //                                   b.F_Left_4,
        //                                   b.F_Left_5,
        //                                   b.F_Left_6,
        //                                   b.F_Left_7,
        //                                   b.F_Left_8,
        //                                   b.F_Left_9,
        //                                   b.F_Left_10,
        //                                   b.R_Left_1,
        //                                   b.R_Left_2,
        //                                   b.R_Left_3,
        //                                   b.R_Left_4,
        //                                   b.R_Left_5,
        //                                   b.R_Left_6,




        //                               }).ToList();
        //                if (buckles.Count > 0)
        //                {

        //                    return Request.CreateResponse(HttpStatusCode.OK, buckles);

        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No buckles found");

        //            }
        //            catch (Exception e)
        //            {


        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
        //            }

        //        }



        //        [HttpGet]
        //        public HttpResponseMessage GetBuckleDistance()
        //        {
        //            try
        //            {


        //                var buckledistance = (from v in db.Vehicles
        //                                      join dist in db.Buckle_Points_Distance on v.vid equals dist.vid
        //                                      select new
        //                                      {
        //                                          v.vid,
        //                                          v.vtype,
        //                                        dist.D_From,
        //                                        dist.D_F_Left_1,

        //                                          dist.D_F_Left_2,

        //                                          dist.D_F_Left_3,
        //                                          dist.D_F_Left_4,
        //                                          dist.D_F_Left_5,
        //                                          dist.D_F_Left_6,
        //                                          dist.D_F_Left_7,
        //                                          dist.D_F_Left_8,
        //                                          dist.D_F_Left_9,
        //                                          dist.D_F_Left_10,
        //                                          dist.D_R_Left_1,
        //                                          dist.D_R_Left_2,
        //                                          dist.D_R_Left_3,
        //                                          dist.D_R_Left_5,
        //                                          dist.D_R_Left_6,




        //                                      }).ToList();


        //                if (buckledistance.Count > 0)
        //                {

        //                    return Request.CreateResponse(HttpStatusCode.OK, buckledistance);

        //                }


        //                return Request.CreateResponse(HttpStatusCode.OK, "no record");

        //            }
        //            catch (Exception ex)
        //            {


        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }




        //        }



        //        [HttpGet]
        //        public HttpResponseMessage GetBuckleDistanceByDFrom(String dfrom)
        //        {
        //            try
        //            {


        //                var buckledistance = (from v in db.Vehicles
        //                                      join dist in db.Buckle_Points_Distance on v.vid equals dist.vid where dist.D_From==dfrom
        //                                      select new
        //                                      {
        //                                          v.vid,
        //                                          v.vtype,
        //                                          dist.D_From,
        //                                          dist.D_F_Left_1,

        //                                          dist.D_F_Left_2,

        //                                          dist.D_F_Left_3,
        //                                          dist.D_F_Left_4,
        //                                          dist.D_F_Left_5,
        //                                          dist.D_F_Left_6,
        //                                          dist.D_F_Left_7,
        //                                          dist.D_F_Left_8,
        //                                          dist.D_F_Left_9,
        //                                          dist.D_F_Left_10,
        //                                          dist.D_R_Left_1,
        //                                          dist.D_R_Left_2,
        //                                          dist.D_R_Left_3,
        //                                          dist.D_R_Left_5,
        //                                          dist.D_R_Left_6,




        //                                      }).ToList();


        //                if (buckledistance.Count > 0)
        //                {

        //                    return Request.CreateResponse(HttpStatusCode.OK, buckledistance);

        //                }


        //                return Request.CreateResponse(HttpStatusCode.OK, "no record");

        //            }
        //            catch (Exception ex)
        //            {


        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }




        //        }


        //        ////************************************* DATA INSERTION FUNCTIONS *********************************************


        //        [HttpPost]


        //        public HttpResponseMessage AddUser(UserData u)
        //        {

        //            try
        //            {

        //                db.UserDatas.Add(u);

        //                db.SaveChanges();

        //                return Request.CreateResponse(HttpStatusCode.OK, "User Added Successfully");

        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }



        //        }
        //        [HttpPost]

        //        public HttpResponseMessage AddVehicle(Vehicle v)
        //        {


        //            try
        //            {

        //                db.Vehicles.Add(v);

        //                db.SaveChanges();

        //                return Request.CreateResponse(HttpStatusCode.OK, "Vehicle Added Successfully");

        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }

        //        }



        //        [HttpPost]

        //        public HttpResponseMessage AddOwnerVehicle(OwnerVehicle ov)
        //        {

        //            try
        //            {

        //                db.OwnerVehicles.Add(ov);

        //                db.SaveChanges();

        //                return Request.CreateResponse(HttpStatusCode.OK, "OwnerVehicle Added Successfully");

        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }





        //        }


        //        [HttpPost]
        //        public HttpResponseMessage AddBucklePoints(BucklePoint bp)
        //        {

        //            try
        //            {

        //                db.BucklePoints.Add(bp);

        //                db.SaveChanges();

        //                return Request.CreateResponse(HttpStatusCode.OK, "BucklePoints  Added Successfully");

        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }





        //        }





        //        [HttpPost]


        //        public HttpResponseMessage AddBucklePointDistance(Buckle_Points_Distance bpd)
        //        {


        //            try
        //            {
        //                bpd.D_From = bpd.vid + "_" + bpd.D_From;
        //                db.Buckle_Points_Distance.Add(bpd);

        //                db.SaveChanges();

        //                return Request.CreateResponse(HttpStatusCode.OK, "BucklePoints  Distance Added Successfully");

        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }





        //        }



        //        [HttpPost]



        //        public HttpResponseMessage AddContacts(Contact cntct)
        //        {


        //            try
        //            {

        //                db.Contacts.Add(cntct);

        //                db.SaveChanges();

        //                return Request.CreateResponse(HttpStatusCode.OK, "Contacts Added Successfully");

        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }






        //        }





        //        [HttpPost]



        //        public HttpResponseMessage AddNotification(Notification noti)
        //        {


        //            try
        //            {

        //                db.Notifications.Add(noti);

        //                db.SaveChanges();

        //                return Request.CreateResponse(HttpStatusCode.OK, "Notification Added Successfully");

        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }





        //        }




        //        ////************************************* DATA Deletion FUNCTIONS *********************************************



        //        [HttpPost]

        //        public HttpResponseMessage DelUser(UserData u)
        //        {
        //            try
        //            {
        //                var usr = db.UserDatas.Find(u.phno);
        //                if (usr != null)
        //                {
        //                    db.UserDatas.Remove(usr);
        //                    db.SaveChanges();
        //                    return Request.CreateResponse(HttpStatusCode.OK, "User with uid=" + u.phno + " is deleted Successfully");
        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No user found....");

        //            }
        //            catch (Exception ex)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }








        //        }




        //        [HttpPost]


        //        public HttpResponseMessage DelVehicle(String vid)
        //        {


        //            try
        //            {

        //                var veh = db.Vehicles.Find(vid);

        //                if (veh != null)
        //                {



        //                    db.Vehicles.Remove(veh);

        //                    db.SaveChanges();
        //                    return Request.CreateResponse(HttpStatusCode.OK, "Vehicle with vid=" + vid + " is deleted Successfully");



        //                }
        //                return Request.CreateResponse(HttpStatusCode.OK, "Vehicle not found.....");

        //            }
        //            catch (Exception ex)
        //            {


        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }




        //        }







        //        [HttpPost]


        //        public HttpResponseMessage DelOwnerVehicle(String  phno)
        //        {

        //            try
        //            {

        //                var owner = db.OwnerVehicles.Find(phno);

        //                if (owner != null)
        //                {


        //                    db.OwnerVehicles.Remove(owner);
        //                    db.SaveChanges();
        //                    return Request.CreateResponse(HttpStatusCode.OK, "Owner with oid=" + phno + " is deleted Successfully");


        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No owner found with oid=" + phno);


        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }



        //        }

        //        [HttpPost]


        //        public HttpResponseMessage DelNotification(String uid)
        //        {

        //            try
        //            {

        //                var noti = db.Notifications.Find(uid);

        //                if (noti!= null)
        //                {


        //                    db.Notifications.Remove(noti);
        //                    db.SaveChanges();
        //                    return Request.CreateResponse(HttpStatusCode.OK, "Notification is deleted Successfully");


        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No Notification found ");


        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }



        //        }


        //        [HttpPost]


        //        public HttpResponseMessage DelContact(String uid)
        //        {

        //            try
        //            {

        //                var cntct = db.Contacts.Find(uid);

        //                if (cntct != null)
        //                {


        //                    db.Contacts.Remove(cntct);
        //                    db.SaveChanges();
        //                    return Request.CreateResponse(HttpStatusCode.OK, "Contact  is deleted Successfully");


        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No Contact found ");


        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }



        //        }


        //        [HttpPost]


        //        public HttpResponseMessage DelBucklePoint(String vid)
        //        {

        //            try
        //            {

        //                var buckle = db.BucklePoints.Find(vid);

        //                if (buckle != null)
        //                {


        //                    db.BucklePoints.Remove(buckle);
        //                    db.SaveChanges();
        //                    return Request.CreateResponse(HttpStatusCode.OK, "Buckle point is deleted Successfully");


        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No BucklePoint found");


        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }



        //        }



        //        [HttpPost]


        //        public HttpResponseMessage DelBucklePointDistance(String Dfrom)
        //        {

        //            try
        //            {

        //                var buckled = db.Buckle_Points_Distance.Find(Dfrom);

        //                if (buckled != null)
        //                {


        //                    db.Buckle_Points_Distance.Remove(buckled);
        //                    db.SaveChanges();
        //                    return Request.CreateResponse(HttpStatusCode.OK, "Buckle point  Distance is deleted Successfully");


        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No BucklePoint Distance found ");


        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //            }



        //        }




        //        ////************************************* DATA Updation FUNCTIONS *********************************************


        //        [HttpPost]



        //        public HttpResponseMessage UpdateUserData(UserData u)
        //        {

        //            try
        //            {

        //                var usr = db.UserDatas.Find(u.phno);
        //                if (usr != null)
        //                {

        //                    db.Entry(usr).CurrentValues.SetValues(u);
        //                    db.SaveChanges();
        //                    return Request.CreateResponse(HttpStatusCode.OK, "Data Updated Successfully....");

        //                }
        //                return Request.CreateResponse(HttpStatusCode.OK, "No Record Found...");


        //            }
        //            catch (Exception ex) { return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message); }




        //        }



        //        [HttpPost]


        //        public HttpResponseMessage UpdateNotification(Notification n)
        //        {


        //            try
        //            {


        //                var noti = db.Notifications.Find(n.uid_phno);

        //                if (noti != null)
        //                {
        //                    db.Entry(noti).CurrentValues.SetValues(n);
        //                    db.SaveChanges();
        //                    return Request.CreateResponse(HttpStatusCode.OK, "Record Updated Successfully...");



        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No Notification Found");


        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message); ;
        //            }





        //        }

        //        [HttpPost]


        //        public HttpResponseMessage UpdateContact(Contact c)
        //        {


        //            try
        //            {


        //                var cntct = db.Contacts.Find(c.uid_phno);

        //                if (cntct != null)
        //                {
        //                    db.Entry(cntct).CurrentValues.SetValues(c);
        //                    db.SaveChanges();
        //                    return Request.CreateResponse(HttpStatusCode.OK, "Record Updated Successfully...");



        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No Contacts Found");


        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message); ;
        //            }





        //        }


        //        [HttpPost]


        //        public HttpResponseMessage UpdateVehicle(Vehicle vh)
        //        {


        //            try
        //            {


        //                var veh = db.Vehicles.Find(vh.vid);

        //                if (veh != null)
        //                {
        //                    db.Entry(veh).CurrentValues.SetValues(vh);
        //                    db.SaveChanges();
        //                    return Request.CreateResponse(HttpStatusCode.OK, "Record Updated Successfully...");



        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No Vehicle Found");


        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message); ;
        //            }





        //        }



        //        [HttpPost]


        //        public HttpResponseMessage UpdateOwnerVehicle(OwnerVehicle ov)
        //        {


        //            try
        //            {


        //                var oveh = db.OwnerVehicles.Find(ov.vid);

        //                if (oveh != null)
        //                {
        //                    db.Entry(oveh).CurrentValues.SetValues(ov);
        //                    db.SaveChanges();
        //                    return Request.CreateResponse(HttpStatusCode.OK, "Record Updated Successfully...");



        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No Owner  Vehicle Found");


        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message); ;
        //            }





        //        }


        //        [HttpPost]


        //        public HttpResponseMessage UpdateBucklePoints(BucklePoint buck)
        //        {


        //            try
        //            {


        //                var buckp = db.Buckle_Points_Distance.Find(buck.vid);

        //                if (buckp != null)
        //                {
        //                    db.Entry(buckp).CurrentValues.SetValues(buck);
        //                    db.SaveChanges();
        //                    return Request.CreateResponse(HttpStatusCode.OK, "Record Updated Successfully...");



        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No BucklePoint Found");


        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message); ;
        //            }





        //        }





        //        [HttpPost]


        //        public HttpResponseMessage UpdateBucklePointsDistance(Buckle_Points_Distance buckpd)
        //        {


        //            try
        //            {


        //                var buckp = db.Buckle_Points_Distance.Find(buckpd.D_From);

        //                if (buckp != null)
        //                {
        //                    db.Entry(buckp).CurrentValues.SetValues(buckpd);
        //                    db.SaveChanges();
        //                    return Request.CreateResponse(HttpStatusCode.OK, "Record Updated Successfully...");



        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No BucklePoint Distance Found");


        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message); ;
        //            }





        //        }

        //        ///////////////////////////////////////////Some Aditional Fuctions ////////////////////////
        //        [HttpGet]
        //        public HttpResponseMessage GetContactsToSendNotification(String uid)
        //        {
        //            try
        //            {
        //                var cntcts = db.Contacts.Where(u =>u.uid_phno == uid).ToList();
        //                if (cntcts.Count > 0)
        //                {
        //                    List<String> send_alert = new List<string>();



        //                    foreach (var c in cntcts)
        //                    {

        //                        var already_accidented = db.Notifications.Where(n => n.phno == c.cphno).ToList();
        //                        if (already_accidented.Count == 0)
        //                        {

        //                            send_alert.Add(c.cphno);


        //                        }




        //                    }
        //                    if (send_alert.Count == 0)
        //                    {
        //                        return Request.CreateResponse(HttpStatusCode.OK, "Cant send Alert");
        //                    }

        //                    return Request.CreateResponse(HttpStatusCode.OK, send_alert);
        //                }


        //                return Request.CreateResponse(HttpStatusCode.OK, "there are no Contacts Related to this Person");

        //            }
        //            catch (Exception ex)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);

        //            }
        //        }


        //        [HttpGet]
        //        public HttpResponseMessage GetAllFrontBuckles(String vid)
        //        {
        //            try
        //            {


        //                var frontBuckles=(from b in db.BucklePoints where b.vid == vid select new {

        //                    b.F_Bumper,b.F_Left_1,
        //                    b.F_Left_2,
        //                    b.F_Left_3,
        //                    b.F_Left_4,
        //                    b.F_Left_5,
        //                    b.F_Left_6,
        //                    b.F_Left_7,
        //                    b.F_Left_8,
        //                    b.F_Left_9,
        //                    b.F_Left_10,





        //                }).ToList();
        //                if (frontBuckles.Count>0) {

        //                    return Request.CreateResponse(HttpStatusCode.OK,frontBuckles);

        //                }

        //                return  Request.CreateResponse(HttpStatusCode.OK,"no record") ;

        //            }
        //            catch (Exception e)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
        //            }


        //        }

        //        [HttpGet]
        //        public HttpResponseMessage GetAllBackBuckles(String vid) { 
        //       try {



        //                var backbuckles=(from b in db.BucklePoints where b.vid==vid select new {


        //                    b.R_Bumper,b.R_Left_1,
        //                    b.R_Left_2,
        //                    b.R_Left_3,
        //                    b.R_Left_4,
        //                    b.R_Left_5,

        //                   b.R_Left_6



        //                }).ToList();
        //                if (backbuckles.Count>0) {


        //                    return Request.CreateResponse(HttpStatusCode.OK,backbuckles);
        //                }
        //                return Request.CreateResponse(HttpStatusCode.OK,"No Record");

        //        }
        //            catch (Exception e)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
        //            }


        //}

        //        [HttpGet]

        //        public HttpResponseMessage GetDoorsBuckles(String vid)
        //        {

        //            try
        //            {
        //                var doors = (from b in db.BucklePoints
        //                             where b.vid == vid 
        //                             select new
        //                             {
        //                                 b.Doors




        //                             }).ToList();
        //                if (doors.Count > 0)
        //                {

        //                    return Request.CreateResponse(HttpStatusCode.OK, doors);

        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No Buckle Points found for this vehicle");



        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);


        //            }



        //        }


        //        [HttpGet]

        //        public HttpResponseMessage GetRoofBuckles(String vid)
        //        {

        //            try
        //            {
        //                var roof = (from b in db.BucklePoints
        //                            where b.vid == vid 
        //                            select new
        //                            {
        //                                b.Roof




        //                            }).ToList();
        //                if (roof.Count > 0)
        //                {

        //                    return Request.CreateResponse(HttpStatusCode.OK, roof);

        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, "No Buckle Points found for this vehicle");



        //            }
        //            catch (Exception ex)
        //            {

        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);


        //            }



        //  }
    }
}
