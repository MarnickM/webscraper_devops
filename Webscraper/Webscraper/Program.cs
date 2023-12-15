using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text.RegularExpressions;
using PushbulletSharp;
using PushbulletSharp.Models.Requests;
using PushbulletSharp.Models.Responses;

namespace Webscraper
{
    class Program
    {
        static void Main(string[] args)
        {
            // make a new ChromeDriver object
            WebDriver driver = new ChromeDriver();

            // Setting up Pushbullet to send a notification to my phone
            //Create client
            var apiKey = "o.bLXA8lPp4DQ5n7rRn3vUEQh8QOAj8iYo";
            // Create a new Pushbullet object to use
            PushbulletClient client = new PushbulletClient(apiKey);

            //Get information about the user account behind the API key
            var currentUserInformation = client.CurrentUsersInformation();

            // XPaths to the following components inside the browser source code
            // YouTube xpaths
            var cookieButton = "/html/body/ytd-app/ytd-consent-bump-v2-lightbox/tp-yt-paper-dialog/div[4]/div[2]/div[6]/div[1]/ytd-button-renderer[1]/yt-button-shape/button/yt-touch-feedback-shape/div/div[2]";
            var searchbox = "/html/body/ytd-app/div[1]/div/ytd-masthead/div[4]/div[2]/ytd-searchbox/form/div[1]/div[1]/input";
            var videoTitles = "/html/body/ytd-app/div[1]/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[2]/ytd-item-section-renderer/div[3]/ytd-video-renderer/div[1]/div/div[1]/div/h3/a/yt-formatted-string";
            var videoPoster = "/html/body/ytd-app/div[1]/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[2]/ytd-item-section-renderer/div[3]/ytd-video-renderer/div[1]/div/div[2]/ytd-channel-name/div/div/yt-formatted-string/a";
            var amountOfViews = "/html/body/ytd-app/div[1]/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[2]/ytd-item-section-renderer/div[3]/ytd-video-renderer/div[1]/div/div[1]/ytd-video-meta-block/div[1]/div[2]/span[1]";
            var thumbnailWithLink = "/html/body/ytd-app/div[1]/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[2]/ytd-item-section-renderer/div[3]/ytd-video-renderer/div[1]/ytd-thumbnail/a";

            // ictjobs xpaths
            var searchboxICT = "/html/body/section/div[1]/div/div[2]/div[1]/div/section/form/div/div/div[3]/div[2]/div[1]/div[1]/div/div[1]/label/input";
            var dateButton = "/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[1]/div[2]/div/div[2]/span[2]/a";
            var jobTitles = "/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[2]/div[1]/div/ul/li/span[2]/a/h2";
            var companies = "/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[2]/div[1]/div/ul/li/span[2]/span[1]";
            var locations = "/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[2]/div[1]/div/ul/li/span[2]/span[2]/span[2]/span/span";
            var keywords = "/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[2]/div[1]/div/ul/li/span[2]/span[3]";
            var jobLinks = "/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[2]/div[1]/div/ul/li/span[2]/a";

            // Playstation xpaths
            // part 1: monthly games
            var freeGamesTitles = "/html/body/div[2]/div/div[1]/div/div[11]/section/div/div[2]/div/div/div/div/div/div/div/div[2]/div/div/div[2]/div/div/h3";
            var freeGamesDescriptions = "/html/body/div[2]/div/div[1]/div/div[11]/section/div/div[2]/div/div/div/div/div/div/div/div[2]/div/div/div[2]/div/div/p";
            var freeGamesBuy = "/html/body/div[2]/div/div[1]/div/div[11]/section/div/div[2]/div/div/div/div/div/div/div/div[2]/div/div/div[3]/div/div/div/a";
            // part 2: search game by name
            var searchButton1 = "/html/body/section/div/header/span/span/button";
            var searchButton2 = "/html/body/div[6]/div/div/div/button[2]";
            var searchbarPSInput = "/html/body/div[6]/div/div/div/div[2]/input";
            var gameTitles = "/html/body/div[2]/div/div[1]/div/div/div/section/div[1]/div[2]/div[3]/div[1]/div/a/div[2]/p";
            var gameGenres = "/html/body/div[2]/div/div[1]/div/div/div/section/div[1]/div[2]/div[3]/div[1]/div/a/div[2]/div";
            var gameLinks = "/html/body/div[2]/div/div[1]/div/div/div/section/div[1]/div[2]/div[3]/div[1]/div/a";

            Console.Clear();
            var applicationOption = "Y";
            while (applicationOption == "Y")
            {
                // set the console color to red
                Console.ForegroundColor = ConsoleColor.Red;
                // make a nice layout and ask the option that the user wants to choose, playing around with the colors
                Console.WriteLine("-----------------------------");
                Console.WriteLine("|                           |");
                Console.WriteLine("|                           |");
                Console.WriteLine("|        Webscraper         |");
                Console.WriteLine("|        by Marnick         |");
                Console.WriteLine("|                           |");
                Console.WriteLine("|                           |");
                Console.WriteLine("-----------------------------");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("What would you like to scrape :)");
                Console.WriteLine("Option 1: YouTube.com");
                Console.WriteLine("Option 2: ictjobs.be");
                Console.WriteLine("Option 3: Playstation.com");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exit: 4");
                // set color back to default
                Console.ResetColor();
                // ask for option
                Console.WriteLine("Your option:");
                var option = Console.ReadLine();

                // initialize a list to gather all data to convert this to csv or json later on
                List<string> listForConvertion = new List<string>();

                // while you don't give a valid option, keep asking for an option and show an error message if the option is not valid
                while (option != "1" && option != "2" && option != "3" && option != "4")
                {
                    // make it clear there is an error by setting the color to red
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sorry, that is an invalid option, please choose between 1,2,3 or 4");
                    // set color to default
                    Console.ResetColor();
                    // ask again
                    Console.WriteLine("Your option:");
                    option = Console.ReadLine();
                }

                if (option == "1")
                {
                    // browse to YouTube.com
                    driver.Navigate().GoToUrl("https://www.youtube.com");
                    // wait 5 sec so that the cookie pop-up can load in
                    Thread.Sleep(5000);

                    // click the "deny all" button inside the cookie pop-up
                    driver.FindElement(By.XPath(cookieButton)).Click();

                    // wait 3 sec for the page to load in
                    Thread.Sleep(3000);

                    // click in the searchbar to start typing our needed elements
                    driver.FindElement(By.XPath(searchbox)).Click();
                    // get the searchbar inside a variable so that I can enter some words and submit it
                    var searchbarYT = driver.FindElement(By.XPath(searchbox));

                    Console.WriteLine("What would you like to see a video about?");
                    Console.WriteLine("");
                    // ask what to search on YouTube via the console
                    var searchTerms = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Here are the 5 newest videos about " + searchTerms + ":");
                    Console.WriteLine("");

                    // enter the needed search terms in the searchbar
                    searchbarYT.SendKeys(searchTerms);
                    // submit and start searching on YouTube
                    searchbarYT.Submit();
                    // change the url so that the filter is enabled that will show the most recent videos
                    driver.Navigate().GoToUrl(driver.Url + "&sp=CAISAhAB");

                    // putting all elements of the objects created by the FindElements() function inside a list so I can easily iterate over them and show the results
                    // so I get the needed data via driver.Findelements() by finding it using the XPath, then I convert this to a list
                    // lastly I put this list inside a variable of the type list<IWebElement>
                    Thread.Sleep(2000);
                    List<IWebElement> titles = driver.FindElements(By.XPath(videoTitles)).ToList();
                    List<IWebElement> videoPosters = driver.FindElements(By.XPath(videoPoster)).ToList();
                    List<IWebElement> views = driver.FindElements(By.XPath(amountOfViews)).ToList();
                    List<IWebElement> videoLinks = driver.FindElements(By.XPath(thumbnailWithLink)).ToList();

                    // initialize a new list where I will put the links of the YouTube videos inside
                    List<string> Links = new List<string>();
                    // loop through the gathered thumbnails and get the attribute "href" which contains the link, then add the link to the new list
                    foreach (var link in videoLinks)
                    {
                        Links.Add(link.GetAttribute("href"));
                    }

                    // add the titles of each column/section for in the json/csv file
                    listForConvertion.Add("Title"); listForConvertion.Add("Channel"); listForConvertion.Add("Views"); listForConvertion.Add("Link"); 
                    listForConvertion.Add("endOfLine");

                    // adding a nice space between my searched words and the found results
                    Console.WriteLine("---------------");
                    // showing the first 5 videos inside my lists and printing their info
                    for (int i = 0; i < 5; i++)
                    {
                        Console.WriteLine($"Title: {titles[i].Text}");
                        Console.WriteLine($"Author: {videoPosters[i].Text}");
                        Console.WriteLine($"Views: {views[i].Text}");
                        Console.WriteLine($"Link: {Links[i]}");
                        Console.WriteLine("---------------");
                        // add the info to the convertion list
                        listForConvertion.Add(titles[i].Text);
                        listForConvertion.Add(videoPosters[i].Text);
                        listForConvertion.Add(views[i].Text);
                        listForConvertion.Add(Links[i]);
                        // after each grouped set of data add an endOfLine that I will use inside the convertion classes to make a json or csv file
                        listForConvertion.Add("endOfLine");
                    }


                }
                else if (option == "2")
                {
                    // browse to ictjobs
                    driver.Navigate().GoToUrl("https://www.ictjob.be/nl/");

                    // wait 5 sec for the page to load in
                    Thread.Sleep(1000);

                    // click on the searchbar to start typing the searchterms
                    driver.FindElement(By.XPath(searchboxICT)).Click();
                    // get the searchbar element via the XPath
                    var searchbarICT = driver.FindElement(By.XPath(searchboxICT));
                    // ask for the searchterms via the console
                    Console.WriteLine("What kind of jobs would you like to find?");
                    Console.WriteLine("");
                    var searchTerms = Console.ReadLine();
                    // enter the searchterms in the searchbar
                    searchbarICT.SendKeys(searchTerms);
                    // submit the searchterms 
                    searchbarICT.Submit();

                    driver.Manage().Window.FullScreen();

                    // click the button to sort on newest jobs
                    driver.FindElement(By.XPath(dateButton)).Click();

                    // set the status to false
                    bool status = false;
                    // while the status is false, get the classes of the date button and convert it to a string
                    while (!status)
                    {
                        var button = driver.FindElement(By.XPath(dateButton)).GetAttribute("class");
                        // use a regular expression to check whether the class "active" is on the button meaning it was clicked
                        bool isActive = Regex.IsMatch(button, "active", RegexOptions.IgnoreCase);
                        // if the class "active" is there end the while loop and wait an additional 10 sec for the sorting to be done
                        if (isActive)
                        {
                            // set the status to true
                            status = true;
                            // the sorting takes a really long time so wait otherwise the unsorted elements will be scraped
                            Thread.Sleep(15000);
                        }
                    }

                    Console.WriteLine("Here are the 5 newest jobs that involve " + searchTerms + ":");
                    // again putting all elements of the objects created by the FindElements() function inside a list so I can easily iterate over them and show the results
                    // so I get the needed data via driver.Findelements() by finding it using the XPath, then I convert this to a list
                    // lastly I put this list inside a variable of the type list<IWebElement>
                    List<IWebElement> jobTitle = driver.FindElements(By.XPath(jobTitles)).ToList();
                    List<IWebElement> company = driver.FindElements(By.XPath(companies)).ToList();
                    List<IWebElement> location = driver.FindElements(By.XPath(locations)).ToList();
                    List<IWebElement> keyword = driver.FindElements(By.XPath(keywords)).ToList();
                    List<IWebElement> jobLink = driver.FindElements(By.XPath(jobLinks)).ToList();

                    // initialize a new list where I will put the links of the YouTube videos inside
                    List<string> linksJobs = new List<string>();
                    // loop through the gathered links and get the attribute "href" which contains the link, then add the link to the new list
                    foreach (var link in jobLink)
                    {
                        linksJobs.Add(link.GetAttribute("href"));
                    }

                    // add the titles of each column/section for in the json/csv file
                    listForConvertion.Add("Job title"); listForConvertion.Add("Company"); listForConvertion.Add("Location"); listForConvertion.Add("Keyword");
                    listForConvertion.Add("Link"); listForConvertion.Add("endOfLine");

                    // showing the first 5 jobs inside my lists and printing their info
                    Console.WriteLine("---------------");
                    for (int i = 0; i < 5; i++)
                    {
                        Console.WriteLine($"Title of job: {jobTitle[i].Text}");
                        Console.WriteLine($"Company: {company[i].Text}");
                        Console.WriteLine($"Located at: {location[i].Text}");
                        Console.WriteLine($"Keywords of this job: {keyword[i].Text}");
                        Console.WriteLine($"The link for the job: {linksJobs[i]}");
                        Console.WriteLine("---------------");
                        // add the info to the convertion list
                        listForConvertion.Add(jobTitle[i].Text);
                        listForConvertion.Add(company[i].Text);
                        listForConvertion.Add(location[i].Text);
                        listForConvertion.Add(keyword[i].Text);
                        listForConvertion.Add(linksJobs[i]);
                        // after each grouped set of data add an endOfLine that I will use inside the convertion classes to make a json or csv file
                        listForConvertion.Add("endOfLine");
                    }
                }
                else if (option == "3")
                {

                    // ask the additional option the user would like to take
                    Console.WriteLine("Would you like to get the new monthly free games (option 1) or search for a specific game (option 2)?");
                    Console.WriteLine("Your option:");
                    // put the given option inside a variable
                    var gameOption = Console.ReadLine();

                    // a status for whether a choice has been made and the scraping has been completed
                    bool completed = false;
                    // as long as the request hasn't been completed keep asking for a gameOption
                    while (!completed)
                    {
                        if (gameOption == "1")
                        {
                            // browse to playstation's whats new page
                            driver.Navigate().GoToUrl("https://www.playstation.com/nl-be/ps-plus/whats-new/");

                            // wait for the site to load in
                            Thread.Sleep(1000);
                            // putting all elements of the objects created by the FindElements() function inside a list
                            List<IWebElement> freeGamesTitle = driver.FindElements(By.XPath(freeGamesTitles)).ToList();
                            List<IWebElement> freeGamesDescription = driver.FindElements(By.XPath(freeGamesDescriptions)).ToList();
                            List<IWebElement> freeGamesCover = driver.FindElements(By.XPath(freeGamesBuy)).ToList();

                            // initialize a new list where I will put the links of the games inside
                            List<string> downloadPages = new List<string>();
                            // loop through the gathered links and get the attribute "href" which contains the link, then add the link to the new list
                            foreach (var link in freeGamesCover)
                            {
                                downloadPages.Add(link.GetAttribute("href"));
                            }

                            // add the titles of each column/section for in the json/csv file
                            listForConvertion.Add("Title"); listForConvertion.Add("Description"); listForConvertion.Add("Link"); listForConvertion.Add("endOfLine");

                            // loop over each element inside each list and show the info
                            // showing a nice layout with seperating lines
                            Console.WriteLine("---------------");
                            for (int i = 0; i < freeGamesTitle.Count(); i++)
                            {
                                Console.WriteLine($"Game title: {freeGamesTitle[i].Text}");
                                Console.WriteLine($"Description: {freeGamesDescription[i].Text}");
                                Console.WriteLine($"Get it: {downloadPages[i]}");
                                Console.WriteLine("---------------");
                                // add the info to the convertion list
                                listForConvertion.Add(freeGamesTitle[i].Text);
                                listForConvertion.Add(freeGamesDescription[i].Text);
                                listForConvertion.Add(downloadPages[i]);
                                // after each grouped set of data add an endOfLine that I will use inside the convertion classes to make a json or csv file
                                listForConvertion.Add("endOfLine");

                                // Sending a small message for each game to my phone using the pushbullet object
                                //Check if useraccount data could be retrieved
                                if (currentUserInformation != null)
                                {
                                    //Create request
                                    PushNoteRequest request = new PushNoteRequest
                                    {
                                        // Send to the account with this email, then define title and body text of the message
                                        Email = currentUserInformation.Email,
                                        Title = freeGamesTitle[i].Text,
                                        Body = "More info: " + downloadPages[i]
                                    };
                                    // Send the message using PushNote() and store the response from the server inside the repsonse variable
                                    PushResponse response = client.PushNote(request);
                                }
                            }
                            // set to true to end the while loop so indicate the scrape has been completed
                            completed = true;
                          
                        }
                        else if (gameOption == "2")
                        {
                            // browse to playstation.com
                            driver.Navigate().GoToUrl("https://www.playstation.com/nl-be/");

                            // wait for the site to load in
                            Thread.Sleep(3000);

                            // click on the searchbar
                            driver.FindElement(By.XPath(searchButton1)).Click();

                            //wait for the site to load 
                            Thread.Sleep(3000);

                            // get the searchbar inside a variable so that I can enter some words and submit it
                            var searchPS = driver.FindElement(By.XPath(searchbarPSInput));

                            Console.WriteLine("What game would you like to search for?");
                            Console.WriteLine("");
                            // ask what to search on Playstation via the console
                            var searchTerms = Console.ReadLine();
                            // clear the console for a clean screen to diplsay the found results on
                            Console.Clear();

                            // enter the needed search terms in the searchbar
                            searchPS.SendKeys(searchTerms);
                            // wait for loading
                            Thread.Sleep(1000);
                            // click on the search icon to start searching, .submit() doesn't work on this form because it is masked
                            driver.FindElement(By.XPath(searchButton2)).Click();

                            // wait for the rsults to load in
                            Thread.Sleep(2000);

                            // putting all elements of the objects created by the FindElements() function inside a list
                            List<IWebElement> gameTitle = driver.FindElements(By.XPath(gameTitles)).ToList();
                            List<IWebElement> gameGenre = driver.FindElements(By.XPath(gameGenres)).ToList();
                            List<IWebElement> gameLink = driver.FindElements(By.XPath(gameLinks)).ToList();

                            // check if there were any items added to the list, if not then the search resulted in nothing
                            if (gameTitle.Any())
                            {
                                // show a clear layout
                                Console.WriteLine("Here are the results I found for " + searchTerms + ":");
                                Console.WriteLine("");
                                // initialize a new list where I will put the links of the info pages for the games inside
                                List<string> Links = new List<string>();
                                // loop through the gathered links and get the attribute "href" which contains the link, then add the link to the new list
                                foreach (var link in gameLink)
                                {
                                    Links.Add(link.GetAttribute("href"));
                                }

                                // add the titles of each column/section for in the json/csv file
                                listForConvertion.Add("Title"); listForConvertion.Add("Genre"); listForConvertion.Add("Link"); listForConvertion.Add("endOfLine");

                                // adding a nice space between my searched words and the found results
                                Console.WriteLine("---------------");
                                // show all the results that were found
                                for (int i = 0; i < gameTitle.Count(); i++)
                                {
                                    Console.WriteLine($"Title: {gameTitle[i].Text}");
                                    Console.WriteLine($"Genre(s): {gameGenre[i].Text}");
                                    Console.WriteLine($"Link for more info: {Links[i]}");
                                    Console.WriteLine("---------------");
                                    // add the info to the convertion list
                                    listForConvertion.Add(gameTitle[i].Text);
                                    listForConvertion.Add(gameGenre[i].Text);
                                    listForConvertion.Add(Links[i]);
                                    // after each grouped set of data add an endOfLine that I will use inside the convertion classes to make a json or csv file
                                    listForConvertion.Add("endOfLine");
                                }
                            }
                            else
                            {
                                // if nothing was found, show this message
                                Console.WriteLine("No results were found.");
                            }
                            // set to true to end the while loop
                            completed = true;

                        }
                        else // when I enter something that is not a 1 or 2, this error message is shown and the program asks me to choose again
                        {
                            Console.WriteLine("Sorry this is an invalid option, please choose between 1 and 2:");
                            gameOption = Console.ReadLine();
                        }
                    }
                }
                // if you choose for the option 4, the porgram will end and show a goodbye note
                else if (option == "4")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Exiting out of the application ... \nThank you for using the Webscraper!");
                    Console.ResetColor();
                    // wait 2 sec for people to read to goodbye message
                    Thread.Sleep(2000);
                    // set the applicationOption to N to stop the main while loop of the entire program
                    applicationOption = "N";
                }
                // if you specify from the start of the program to immediately stop (so no scraping done), don't ask to save data to json or csv
                if (option != "4")
                {
                    // if you scraped something, ask to save it in a json or csv file
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Would you like to save your results to a JSON or CSV file, type \"Y\" to confirm or any other key to skip");
                    Console.ResetColor();
                    // store the response in a variable, Y or N
                    var fileConversionOption = Console.ReadLine();
                    // if you saud to save it, so Y, then pass the list containing all the scraped data to the choice class for processing in either json or csv
                    if (fileConversionOption == "Y")
                    {
                        ConvertTo.Choice(listForConvertion);
                    }
                    else
                    {
                        // show a message that the data won't be saved since you didn't say Y
                        // give it a red text color
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Results won't be saved...\n");
                        Console.ResetColor();
                    }
                }
                // clear the convertion list before doing another scrape (otherwise the scraped data from the previous scrape will still be present)
                listForConvertion.Clear();
                // ask if you want to scrape anything else
                // if the application option is still Y, ask to continue
                if (applicationOption == "Y")
                {
                    // ask to continue
                    Console.WriteLine("Would you like to continue? If so type \"Y\", else type \"N\"");
                    applicationOption = Console.ReadLine();
                    // if you give an anwser that isn't equal to Y or N, ask again untill you give a correct anwser
                    while (applicationOption != "Y" && applicationOption != "N")
                    {
                        // say that the anwser is not valid and ask again
                        // changing color so that it is clear that there is a mistake
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Sorry, that is an invalid option, please choose between \"Y\" or \"N\"");
                        // return color to default
                        Console.ResetColor();
                        // get the new anwser
                        applicationOption = Console.ReadLine();
                    }

                    // sending a goodbye message when the user wants to stop when you immediatley choose to stop 
                    if (applicationOption == "N")
                    {
                        // set color to green and show a nice message
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Exiting out of the application ... \nThank you for using the Webscraper!");
                        Console.ResetColor();
                        // wait for person to read message
                        Thread.Sleep(2000);
                        // set applicationOption to N to stop main while loop of program
                        applicationOption = "N";
                    }

                    // close the browser
                    driver.Close();
                    // start a new browser session
                    driver = new ChromeDriver();
                    // clear the console
                    Console.Clear();
                }
                // clear the console
                Console.Clear();
            }
        }
    }
}
