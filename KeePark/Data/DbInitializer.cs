using KeePark.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeePark.Data
{
    public static class DbInitializer
    {
        public static void Initialize(KeeParkContext context, IdentityContext idcontext)
        {
            context.Database.EnsureCreated();
            idcontext.Database.EnsureCreated();

            var users = new GeneralUser[]
            {
                new GeneralUser{UserName="mon@mon.com",NormalizedUserName="MON@MON.COM",Email="mon@mon.com",NormalizedEmail="MON@MON.COM",PhoneNumber="0674567283",UID="123456789",FirstName="Monica",LastName="Geller",
                             CreditCard="172889829",CarNumber="6666555",CarType="bmw",Address="Central Perk",Balance=100,History="2,6,6,10"},
                new GeneralUser{UserName="geller@geller.com",NormalizedUserName="GELLER@GELLER.COM",Email="geller@geller.com",NormalizedEmail="GELLER@GELLER.COM",PhoneNumber="0674567298",UID="777777777",FirstName="Ross",LastName="Geller",
                             CreditCard="1652672891",CarNumber="5555555",CarType="porche",Address="Central Perk",Balance=400,History="12,12,12"},
                new GeneralUser{UserName="green@green.com",NormalizedUserName="GREEN@GREEN.COM",Email="green@green.com",NormalizedEmail="GREEN@GREEN.COM",PhoneNumber="0676767298",UID="676767676",FirstName="Rachel",LastName="Green",
                             CreditCard="33356782",CarNumber="7777772",CarType="bentli",Address="Central Perk",Balance=200, History="10"},
                new GeneralUser{UserName="jj@jj.com",NormalizedUserName="JJ@JJ.COM",Email="jj@jj.com",NormalizedEmail="JJ@JJ.COM",PhoneNumber="0567487654",UID="310070654",FirstName="Joe",LastName="Tribbiani",
                             CreditCard="356262829",CarNumber="2345671",CarType="porche",Address="Central Perk",Balance=50, History="1,8"},
                new GeneralUser{UserName="bing@bing.com",NormalizedUserName="BING@BING.COM",Email="bing@bing.com",NormalizedEmail="BING@BING.COM",PhoneNumber="0524897653",UID="220022622",FirstName="Chandler",LastName="Bing",
                             CreditCard="567666663",CarNumber="1234222",CarType="bmw",Address="Central Perk",Balance=700,History="1,3,5,6"},
                new GeneralUser{UserName="p@p.com",NormalizedUserName="P@P.COM",Email="p@p.com",NormalizedEmail="P@P.COM",PhoneNumber="0676767298",UID="310088888",FirstName="Pheabe",LastName="Buffe",
                             CreditCard="23333452",CarNumber="7777898",CarType="mercedes",Address="Central Perk",Balance=100, History="1,2,3,11,11"},
                new GeneralUser{UserName="cris@cris.com",NormalizedUserName="CRIS@CRIS.COM",Email="cris@cris.com",NormalizedEmail="CRIS@CRIS.COM",PhoneNumber="0534567832",UID="454647483",FirstName="Cristiano",LastName="Ronaldo",
                             CreditCard="7777777",CarNumber="7777777",CarType="aston martin",Address="Madrid",Balance=1000,History="3,6"},
                new GeneralUser{UserName="ramos@ramos.com",NormalizedUserName="RAMOS@RAMOS.COM",Email="ramos@ramos.com",NormalizedEmail="RAMOS@RAMOS.COM",PhoneNumber="0676767298",UID="144407652",FirstName="Sergio",LastName="Ramos",
                             CreditCard="4444444",CarNumber="4444444",CarType="bmw",Address="Madrid",Balance=900, History="4,9,11,11,12,12"},
                new GeneralUser{UserName="toni@toni.com",NormalizedUserName="TONI@TONI.COM",Email="toni@toni.com",NormalizedEmail="TONI@TONI.COM",PhoneNumber="0546783547",UID="310078543",FirstName="Toni",LastName="Stark",
                             CreditCard="747478993",CarNumber="9808778",CarType="porche",Address="New York",Balance=2000},
                new GeneralUser{UserName="rogers@rogers.com",NormalizedUserName="ROGERS@ROGERS.COM",Email="rogers@rogers.com",NormalizedEmail="ROGERS@ROGERS.COM",PhoneNumber="0534567876",UID="978850654",FirstName="Steve",LastName="Rogers",
                             CreditCard="45673829",CarNumber="66667882",CarType="lamborgini",Address="New York",Balance=900}
              //  new GeneralUser{UserName="peter@peter.com",NormalizedUserName="PETER@PETER.COM",Email="peter@peter.com",NormalizedEmail="PETER@PETER.COM",PhoneNumber="0547654332",UID="326680978",FirstName="Peter",LastName="Parker",
                //             CreditCard="23466783",CarNumber="2367892",CarType="bmw",Address="New York",Balance=900, History="5,5,5,6,8,8,8,10"}
            };
            foreach (GeneralUser u in users)
            {
                idcontext.GeneralUser.Add(u);
            }
            idcontext.SaveChanges(); 
            



            var spots = new ParkingSpot[]
            {
                new ParkingSpot{ParkingSpotID=1,SpotName="Soho",OwnerID="123456789",Address="Rishon Lezion",Price=21,NunOfOrders=3,SpotDescription="foodi",filePath="3bdd1af3-ce4c-4a40-9fcd-2f0f44006134_1.jpg" },
                new ParkingSpot{ParkingSpotID=2,SpotName="TYO",OwnerID="123456789",Address="Rotchild 12, Tel Aviv",Price=25,NunOfOrders=2,SpotDescription="sushi",filePath="c0740792-8da0-4401-b7b9-c2e5bc78af80_2.jpg" },
                new ParkingSpot{ParkingSpotID=3,SpotName="Collman",OwnerID="777777777",Address="eli wisel, Rishon Lezion",Price=5,NunOfOrders=3,SpotDescription="aducation",filePath="bf51674f-7dee-4af8-a276-1af430a1129d_3.jpg" },
                new ParkingSpot{ParkingSpotID=4,SpotName="Namos",OwnerID="326680978",Address="Marina Herzeliya",Price=21,NunOfOrders=1,SpotDescription="yammi",filePath="116ebee8-4da4-4700-8ea4-14ee0ecd4d1f_4.jpg"},
                new ParkingSpot{ParkingSpotID=5,SpotName="Ynet",OwnerID="310078543",Address="Noah Moses 1, Rishon LeZion",Price=8,NunOfOrders=4,SpotDescription="news", filePath="ed1f45d0-1a82-4665-8c89-bdc60ccaca10_5.jpg" },
                new ParkingSpot{ParkingSpotID=6,SpotName="FU",OwnerID="310078543",Address="Dizingoff 22, Tel Aviv",Price=14,NunOfOrders=5,SpotDescription="sushi", filePath="a07d896c-fc6e-487f-a468-459006a831ac_6.jpg" },
                new ParkingSpot{ParkingSpotID=7,SpotName="Turkiz",OwnerID="310078543",Address="Tel Aviv",Price=21,NunOfOrders=0,SpotDescription="food",filePath="0c183b35-a957-4da1-a3eb-de1953a2e1d2_7.jpg" },
                new ParkingSpot{ParkingSpotID=8,SpotName="Blumffild",OwnerID="454647483",Address="Jerusalem Boulevard, Jaffa",Price=18,NunOfOrders=4,SpotDescription="football",filePath="596c163e-0724-4397-b54e-9dc64e104401_8.jpg" },
                new ParkingSpot{ParkingSpotID=9,SpotName="CR7",OwnerID="454647483",Address="Ashdod Port",Price=18,NunOfOrders=1,SpotDescription="clothing",filePath="23fd7ece-a535-4ebc-9be0-03c198bc12b9_9.jpg" },
                new ParkingSpot{ParkingSpotID=10,SpotName="Segev",OwnerID="454647483",Address="Hazahav Mall, Rishon Lezion",Price=5,NunOfOrders=3,SpotDescription="food",filePath="05293d23-7df6-4a5f-9326-58740474b774_10.jpg" },
                new ParkingSpot{ParkingSpotID=11,SpotName="Regina",OwnerID="326680978",Address="Mammilla, Jerusalem",Price=18,NunOfOrders=4,SpotDescription="clothing",filePath="80fbf5eb-9a82-499f-9116-56d2f782d9a6_11.jpg" },
                new ParkingSpot{ParkingSpotID=12,SpotName="Anita",OwnerID="454647483",Address="Neve Tzedek, Tel Aviv",Price=12,NunOfOrders=5,SpotDescription="ice cream",filePath="eca40f9a-22a7-4079-bb5f-c4ddaf1e2fe6_12.jpg" }
            };

            foreach (ParkingSpot p in spots)
            {
                context.ParkingSpot.Add(p);
            }
          context.SaveChanges(); 

            var reservations = new ReserveSpot[]
            {
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="310070654",SpotID=spots.Single(n=>n.SpotName=="Soho").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-25"),ReservationDate=DateTime.Parse("2019-11-11"),ReservationHour=12,Duration=2,carNumber="343434"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="220022622",SpotID=spots.Single(n=>n.SpotName=="Soho").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-25"),ReservationDate=DateTime.Parse("2019-10-21"),ReservationHour =22,Duration=2,carNumber="222222"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="310088888",SpotID=spots.Single(n=>n.SpotName=="Soho").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-23"),ReservationHour=16,Duration=3,carNumber="121212"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="310088888",SpotID=spots.Single(n=>n.SpotName=="TYO").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-30"),ReservationHour=16,Duration=3,carNumber="121212"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="123456789",SpotID=spots.Single(n=>n.SpotName=="TYO").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-23"),ReservationHour=16,Duration=3,carNumber="666666"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="220022622",SpotID=spots.Single(n=>n.SpotName=="Collman").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-12-21"),ReservationHour=12,Duration=1,carNumber="575757"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="454647483",SpotID=spots.Single(n=>n.SpotName=="Collman").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-08"),ReservationHour=20,Duration=3,carNumber="999999"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="310088888",SpotID=spots.Single(n=>n.SpotName=="Collman").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-12-29"),ReservationHour=19,Duration=1,carNumber="121212"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="144407652",SpotID=spots.Single(n=>n.SpotName=="Namos").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-12-29"),ReservationHour=19,Duration=1,carNumber="989898"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="326680978",SpotID=spots.Single(n=>n.SpotName=="Ynet").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-12-22"),ReservationHour=19,Duration=1,carNumber="2367892"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="326680978",SpotID=spots.Single(n=>n.SpotName=="Ynet").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-12-21"),ReservationHour=19,Duration=1,carNumber="2367892"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="326680978",SpotID=spots.Single(n=>n.SpotName=="Ynet").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-12-20"),ReservationHour=19,Duration=1,carNumber="2367892"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="220022622",SpotID=spots.Single(n=>n.SpotName=="Ynet").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-09"),ReservationHour=9,Duration=3,carNumber="222222"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="220022622",SpotID=spots.Single(n=>n.SpotName=="FU").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-25"),ReservationDate=DateTime.Parse("2019-11-20"),ReservationHour=19,Duration=2,carNumber="222222"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="454647483",SpotID=spots.Single(n=>n.SpotName=="FU").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-25"),ReservationDate=DateTime.Parse("2019-11-20"),ReservationHour=21,Duration=3,carNumber="575757"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="326680978",SpotID=spots.Single(n=>n.SpotName=="FU").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-24"),ReservationDate=DateTime.Parse("2019-11-30"),ReservationHour=22,Duration=2,carNumber="2367892"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="123456789",SpotID=spots.Single(n=>n.SpotName=="FU").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-23"),ReservationDate=DateTime.Parse("2019-12-25"),ReservationHour=16,Duration=2,carNumber="6666555"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="123456789",SpotID=spots.Single(n=>n.SpotName=="FU").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-07"),ReservationHour=20,Duration=3,carNumber="6666555"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="326680978",SpotID=spots.Single(n=>n.SpotName=="Blumffild").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-10"),ReservationHour=18,Duration=3,carNumber="2367892"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="326680978",SpotID=spots.Single(n=>n.SpotName=="Blumffild").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-15"),ReservationHour=19,Duration=3,carNumber="2367892"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="326680978",SpotID=spots.Single(n=>n.SpotName=="Blumffild").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-12-27"),ReservationHour=19,Duration=3,carNumber="2367892"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="310070654",SpotID=spots.Single(n=>n.SpotName=="Blumffild").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-12-02"),ReservationHour=19,Duration=3,carNumber="2345671"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="144407652",SpotID=spots.Single(n=>n.SpotName=="CR7").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-08"),ReservationHour=13,Duration=3,carNumber="4444444"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="777777777",SpotID=spots.Single(n=>n.SpotName=="Anita").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-05"),ReservationHour=18,Duration=1,carNumber="5555555"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="777777777",SpotID=spots.Single(n=>n.SpotName=="Anita").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-15"),ReservationHour=23,Duration=1,carNumber="5555555"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="777777777",SpotID=spots.Single(n=>n.SpotName=="Anita").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-22"),ReservationHour=22,Duration=2,carNumber="5555555"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="310088888",SpotID=spots.Single(n=>n.SpotName=="Regina").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-10-15"),ReservationHour=19,Duration=3,carNumber="2345623"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="310088888",SpotID=spots.Single(n=>n.SpotName=="Regina").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-10-29"),ReservationHour=15,Duration=2,carNumber="2345623"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="144407652",SpotID=spots.Single(n=>n.SpotName=="Regina").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-09-10"),ReservationHour=18,Duration=2,carNumber="8765578"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="144407652",SpotID=spots.Single(n=>n.SpotName=="Regina").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-11"),ReservationHour=13,Duration=3,carNumber="8765578"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="144407652",SpotID=spots.Single(n=>n.SpotName=="Anita").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-16"),ReservationHour=21,Duration=1,carNumber="5555555"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="144407652",SpotID=spots.Single(n=>n.SpotName=="Anita").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-09-09"),ReservationHour=21,Duration=1,carNumber="5555555"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="326680978",SpotID=spots.Single(n=>n.SpotName=="Segev").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-09-01"),ReservationHour=20,Duration=3,carNumber="2367892"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="123456789",SpotID=spots.Single(n=>n.SpotName=="Segev").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-10-09"),ReservationHour=18,Duration=2,carNumber="1212212"},
                new ReserveSpot{ReserveSpotID=new Guid(),UserID="676767676",SpotID=spots.Single(n=>n.SpotName=="Segev").ParkingSpotID,CreatedOn=DateTime.Parse("2019-10-26"),ReservationDate=DateTime.Parse("2019-11-12"),ReservationHour=20,Duration=3,carNumber="7777772"}
            };
            foreach (ReserveSpot r in reservations)
            {
                context.ReserveSpot.Add(r);
            }
            try { context.SaveChanges(); } catch { }
            

        }
    }
}
