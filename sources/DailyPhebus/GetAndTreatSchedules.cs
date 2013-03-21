using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;

namespace DailyPhebus
{
    class GetAndTreatSchedules
    {

        private ServerData sd;


        public GetAndTreatSchedules() 
        {
            sd = new ServerData();
        }

        public List<BusLine> getBusLines()
        {
            List<BusLine> resultList = new List<BusLine>();
            try
            {
                JObject linesObject = JObject.Parse(sd.sendAndLoad("mode=getBusLines"));
                var lines = from p in linesObject["busLineList"].Children()
                            select new { letter = p["letter"], fe = p["firstExtremity"], se = p["secondExtremity"] };

                resultList.Add(BusLine.Zero);
                foreach (var line in lines)
                    resultList.Add(new BusLine(line.letter.ToString(), line.fe.ToString(), line.se.ToString()));

            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return resultList;
        }




        public List<BusStop> getBusStops(BusLine busLine)
        {
            if (busLine == null)
                return null;

            List<BusStop> resultList = new List<BusStop>();
            try
            {
                JObject stopsObject = JObject.Parse(sd.sendAndLoad("mode=getBusStops&letter=" + busLine.letter));
                var stops = from p in stopsObject["busStopList"].Children()
                            select new { name = p["name"], code = p["code"] };

                foreach (var stop in stops)
                    resultList.Add(new BusStop(stop.name.ToString(), stop.code.ToString()));

            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return resultList;
        }



        public string getBusSchedules(LoadForm.LoadType type, BusLine busLine, BusStop busStop, String date, String hour, short direction)
        {
            string hourQuery = "&hour=";
            if (type == LoadForm.LoadType.HourlySchedules)
                hourQuery = "&hour=" + hour;

            System.Console.WriteLine("mode=getBusSchedules&letter=" + busLine.letter + "&codeBusStop=" + busStop.code + "&direction=" + direction + "&date=" + date + hourQuery);
            return sd.sendAndLoad("mode=getBusSchedules&letter=" + busLine.letter + "&codeBusStop=" + busStop.code + "&direction=" + direction + "&date=" + date + hourQuery);
        }

    }
}
