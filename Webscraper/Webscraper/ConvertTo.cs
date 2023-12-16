using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace Webscraper
{
    class ConvertTo
    {
        // class that will ask for a choice between json and csv
        // it will get a list containing the scraped data and pass it on to the correct function
        public static void Choice(List<string> data)
        {
            // define the choice variable
            var choice = "";
            // while the choice doesn't match any of the given options, keep asking and show error message
            while (choice != "1" && choice != "2")
            {
                // ask for the choice (set color to yellow)
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Please specify which type: 1 for JSON and 2 for CSV");
                // set color to default
                Console.ResetColor();
                // choice is put in variable
                choice = Console.ReadLine();
                // if choice = 1 => json, choice = 2 => csv, else show error message and ask again
                if (choice == "1")
                {
                    // pass list to json function
                    Json(data);
                }
                else if (choice == "2")
                {
                    // pass list to csv function
                    Csv(data);
                } 
                else
                {
                    // set color to red for error message and display it, afterwards reset color
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sorry, that option is invalid, please choose between 1 and 2");
                    Console.ResetColor();
                }
            }
        }

        // function that will write given list to a JSON file
        public static void Json(List<string> data)
        {
            // define a dictionary called jsonList with string items inside of it
            Dictionary<string, string> jsonList = new Dictionary<string, string>();
            // define a list that will hold a dictionary inside of it
            List<Dictionary<string, string>> DictionaryList = new List<Dictionary<string, string>>();

            // define the start point of the data inside the recieved list since I added certain headers followed by the first endOfLine
            int startOfData = 0;
            foreach (string i in data)
            {
                // keep going until we find the first endOfLine and store the position if this data inside the variable startOfData
                if (i == "endOfLine") {
                    break;
                }
                // increase as long as we don't encounter an endOfLine
                startOfData++;
            }
            // define a new counter to get the matching header from the first items before my first endOfLine
            int count = 0;
            // now use the start position of the data we found eralier to make a for loop that will start at the needed data and skip the headers I added
            // startOfData + 1 otherwise we start at the endOfLine and go for the entire length of the data list
            for (int x = startOfData + 1; x < data.Count(); x++)
            {
                if (data[x] == "endOfLine")
                {
                    // when we reach an endOfLine, reset the counter because we will have to add a new line and start again with title, channel, ....
                    count = 0;
                    // add the current jsonList to a dictionary
                    DictionaryList.Add(jsonList);
                    // empty the jsonList to fill it again by setting it equal to an empty list (define a new list) jsonList.clear() also does this but by adding this
                    // to the dictionary, this gets linked and if I use the .clear() function, the contents of my dicionary are also cleared
                    // and that is something I obviously don't want
                    jsonList = new Dictionary<string, string>();
                }
                else
                {
                    // as long as we don't encounter an endOfLine, add the data to our jsonList
                    // the structure of the recieved data list is: .......data.......endOfLine.......data.......endOfLine............
                    // in my data list, the first items are the headers, after this the corresponding data comes in the same order:
                    // (Title,Channel,Views,Link,endOfLine,title1, channel name1,views1, link1, endOfLine,......,link5,endOfLine)
                    // so now I am putting each piece of data in the jsonList
                    // so here I am adding a key:value pair for each piece of data
                    // example:
                    // [{"Title":"title of video","Channel":"channel name","Views":"number of views","Link":......
                    // use the count to get the corresponding header for each data field and add to the json
                    jsonList.Add(data[count],data[x]);
                    // inscrease counter
                    count++;
                }
            }
            // what we have created now if for example our recieved list with data has 5 main elements (example: 5 scraped videos with each video having a title, channel,...)
            // is a list that has a length of 5 and each item in that list is a dictionary holding title, channel, views,... for each video in key:value pairs

            // in order to write this dictionary to a file we need to convert it to a json object
            string json = JsonConvert.SerializeObject(DictionaryList);
            // define a path to a file to which we want to write our json
            string nameFile = Path.Combine(@"C:\Users\Marni\Desktop\Thomas More\Jaar 2\Devops&Security\project\JSON_file.json");

            // create a StreamWriter object and give the file path we just defined to this object
            // the false parameter states that if the files doesn't exist, create it else if the file exists, overwrite it
            using (StreamWriter file = new StreamWriter(nameFile, false))
            {
                // write the content of the json variable to the file (string)
                file.WriteLine(json.ToString());
                // after it has been written to the file, close it
                file.Close();
            }
            // show a message in the console with the color yellow that indicates that the json was written to a file by giving the path
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("A JSON file has been made at this location: C:\\Users\\Marni\\Desktop\\Thomas More\\Jaar 2\\Devops&Security\\project\\JSON_file.json");
            // set color to default
            Console.ResetColor();
        }

        // function that will write given list to a CSV file
        public static void Csv(List<string> data)
        {
            // define a path to a file and store it in a variable
            string nameFile = Path.Combine(@"C:\Users\Marni\Desktop\Thomas More\Jaar 2\Devops&Security\project\CSV_file.csv");
            // create a StreamWriter object and give the file path we just defined to this object
            // the false parameter states that if the files doesn't exist, create it else if the file exists, overwrite it
            using (StreamWriter file = new StreamWriter(nameFile, false))
            {
                // write each piece of data to the file, loop over everything
                foreach (string i in data)
                {
                    // if we encounter an endOfLine, use file.WriteLine() to "write" an ENTER and go to the next line
                    if (i == "endOfLine")
                    {
                        file.WriteLine();
                    }
                    else
                    {
                        // write every single item in our data followed by a comma
                        file.Write($"\"{i}\"" + ", ");
                    }
                }
            }
            // show a message in the console with the color yellow that indicates that the json was written to a file by giving the path
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("A CSV file has been made at this location: C:\\Users\\Marni\\Desktop\\Thomas More\\Jaar 2\\Devops&Security\\project\\CSV_file.csv");
            // set color to default
            Console.ResetColor();
        }
    }
}
