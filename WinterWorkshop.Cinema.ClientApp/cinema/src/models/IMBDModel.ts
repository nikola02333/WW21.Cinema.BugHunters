export interface IMDB{
  data:        Data;
  youtubeData: YoutubeData;
}
export interface YoutubeData {
  imDbId:       string;
  title:        string;
  fullTitle:    string;
  type:         string;
  year:         string;
  videoId:      string;
  videoUrl:     string;
  errorMessage: string;
}

export interface Data {
    id:               string;
    title:            string;
    originalTitle:    string;
    fullTitle:        string;
    type:             string;
    year:             string;
    image:            string;
    releaseDate:      Date;
    runtimeMins:      string;
    runtimeStr:       string;
    plot:             string;
    plotLocal:        string;
    plotLocalIsRtl:   boolean;
    awards:           string;
    directors:        string;
    directorList:     CompanyListElement[];
    writers:          string;
    writerList:       CompanyListElement[];
    stars:            string;
    starList:         CompanyListElement[];
    actorList:        ActorList[];
    fullCast:         null;
    genres:           string;
    genreList:        CountryListElement[];
    companies:        string;
    companyList:      CompanyListElement[];
    countries:        string;
    countryList:      CountryListElement[];
    languages:        string;
    languageList:     CountryListElement[];
    contentRating:    string;
    imDbRating:       string;
    imDbRatingVotes:  string;
    metacriticRating: string;
    ratings:          Ratings;
    wikipedia:        null;
    posters:          null;
    images:           null;
    trailer:          Trailer;
    boxOffice:        BoxOffice;
    tagline:          string;
    keywords:         string;
    keywordList:      string[];
    similars:         Similar[];
    tvSeriesInfo:     null;
    tvEpisodeInfo:    null;
    errorMessage:     string;
}

export interface ActorList {
    id:          string;
    image:       string;
    name:        string;
    asCharacter: string;
}

export interface BoxOffice {
    budget:                   string;
    openingWeekendUSA:        string;
    grossUSA:                 string;
    cumulativeWorldwideGross: string;
}

export interface CompanyListElement {
    id:   string;
    name: string;
}

export interface CountryListElement {
    key:   string;
    value: string;
}



export interface Ratings {
    imDbId:         string;
    title:          string;
    fullTitle:      string;
    type:           string;
    year:           string;
    imDb:           string;
    metacritic:     string;
    theMovieDb:     string;
    rottenTomatoes: string;
    tV_com:         string;
    filmAffinity:   string;
    errorMessage:   string;
}

export interface Similar {
    id:         string;
    title:      string;
    fullTitle:  string;
    year:       string;
    image:      string;
    plot:       string;
    directors:  string;
    stars:      string;
    genres:     string;
    imDbRating: string;
}

export interface Trailer {
    imDbId:           string;
    title:            string;
    fullTitle:        string;
    type:             string;
    year:             string;
    videoId:          string;
    videoTitle:       string;
    videoDescription: string;
    thumbnailUrl:     string;
    uploadDate:       string;
    link:             string;
    linkEmbed:        string;
    errorMessage:     string;
}

export const imdbData:IMDB = {
   data: {
        "id": "tt0371746",
        "title": "Iron Man",
        "originalTitle": "",
        "fullTitle": "Iron Man (2008)",
        "type": "Movie",
        "year": "2008",
        "image": "https://imdb-api.com/images/original/MV5BMTczNTI2ODUwOF5BMl5BanBnXkFtZTcwMTU0NTIzMw@@._V1_Ratio0.6791_AL_.jpg",
        "releaseDate": new Date("2008-04-30"),
        "runtimeMins": "126",
        "runtimeStr": "2h 6mins",
        "plot": "Tony Stark. Genius, billionaire, playboy, philanthropist. Son of legendary inventor and weapons contractor Howard Stark. When Tony Stark is assigned to give a weapons presentation to an Iraqi unit led by Lt. Col. James Rhodes, he's given a ride on enemy lines. That ride ends badly when Stark's Humvee that he's riding in is attacked by enemy combatants. He survives - barely - with a chest full of shrapnel and a car battery attached to his heart. In order to survive he comes up with a way to miniaturize the battery and figures out that the battery can power something else. Thus Iron Man is born. He uses the primitive device to escape from the cave in Iraq. Once back home, he then begins work on perfecting the Iron Man suit. But the man who was put in charge of Stark Industries has plans of his own to take over Tony's technology for other matters.",
        "plotLocal": "",
        "plotLocalIsRtl": false,
        "awards": "Nominated for 2 Oscars. Another 22 wins & 70 nominations.",
        "directors": "Jon Favreau",
        "directorList": [
          {
            "id": "nm0269463",
            "name": "Jon Favreau"
          }
        ],
        "writers": "Mark Fergus, Hawk Ostby, Art Marcum, Matt Holloway, Stan Lee, Don Heck, Larry Lieber, Jack Kirby",
        "writerList": [
          {
            "id": "nm1318843",
            "name": "Mark Fergus"
          },
          {
            "id": "nm1319757",
            "name": "Hawk Ostby"
          },
          {
            "id": "nm1436466",
            "name": "Art Marcum"
          },
          {
            "id": "nm0391344",
            "name": "Matt Holloway"
          },
          {
            "id": "nm0498278",
            "name": "Stan Lee"
          },
          {
            "id": "nm1411347",
            "name": "Don Heck"
          },
          {
            "id": "nm1293367",
            "name": "Larry Lieber"
          },
          {
            "id": "nm0456158",
            "name": "Jack Kirby"
          }
        ],
        "stars": "Robert Downey Jr., Gwyneth Paltrow, Terrence Howard, Jeff Bridges",
        "starList": [
          {
            "id": "nm0000375",
            "name": "Robert Downey Jr."
          },
          {
            "id": "nm0000569",
            "name": "Gwyneth Paltrow"
          },
          {
            "id": "nm0005024",
            "name": "Terrence Howard"
          },
          {
            "id": "nm0000313",
            "name": "Jeff Bridges"
          }
        ],
        "actorList": [
          {
            "id": "nm0000375",
            "image": "https://imdb-api.com/images/original/MV5BNzg1MTUyNDYxOF5BMl5BanBnXkFtZTgwNTQ4MTE2MjE@._V1_Ratio0.7273_AL_.jpg",
            "name": "Robert Downey Jr.",
            "asCharacter": "Tony Stark"
          },
          {
            "id": "nm0005024",
            "image": "https://imdb-api.com/images/original/MV5BMTk3NTY4NzgyOV5BMl5BanBnXkFtZTcwODEzNTkxNg@@._V1_Ratio0.8182_AL_.jpg",
            "name": "Terrence Howard",
            "asCharacter": "Rhodey"
          },
          {
            "id": "nm0000313",
            "image": "https://imdb-api.com/images/original/MV5BNTU1NjM4MDYzMl5BMl5BanBnXkFtZTcwMjIwMjMyMw@@._V1_Ratio0.7273_AL_.jpg",
            "name": "Jeff Bridges",
            "asCharacter": "Obadiah Stane"
          },
          {
            "id": "nm0000569",
            "image": "https://imdb-api.com/images/original/MV5BNzIxOTQ1NTU1OV5BMl5BanBnXkFtZTcwMTQ4MDY0Nw@@._V1_Ratio0.7273_AL_.jpg",
            "name": "Gwyneth Paltrow",
            "asCharacter": "Pepper Potts"
          },
          {
            "id": "nm0004753",
            "image": "https://imdb-api.com/images/original/MV5BMTg2NzAzNzE5N15BMl5BanBnXkFtZTcwMjMyODM0MQ@@._V1_Ratio0.7727_AL_.jpg",
            "name": "Leslie Bibb",
            "asCharacter": "Christine Everhart"
          },
          {
            "id": "nm0869467",
            "image": "https://imdb-api.com/images/original/MV5BZjVmYTE0ZmItYzZhMC00OTA2LWIxYWEtYjQ4NGQ2OTQyZGY2XkEyXkFqcGdeQXVyMTE2MzE4NzQ@._V1_Ratio0.7727_AL_.jpg",
            "name": "Shaun Toub",
            "asCharacter": "Yinsen"
          },
          {
            "id": "nm0846687",
            "image": "https://imdb-api.com/images/original/MV5BMjA0OTc2OTg1OV5BMl5BanBnXkFtZTcwNzY2NDA3MQ@@._V1_Ratio0.7273_AL_.jpg",
            "name": "Faran Tahir",
            "asCharacter": "Raza"
          },
          {
            "id": "nm0163988",
            "image": "https://imdb-api.com/images/original/MV5BMjYyNjAwNDUyOV5BMl5BanBnXkFtZTgwOTc5NzgyNjE@._V1_Ratio0.7273_AL_.jpg",
            "name": "Clark Gregg",
            "asCharacter": "Agent Coulson"
          },
          {
            "id": "nm0810488",
            "image": "https://imdb-api.com/images/original/MV5BZmQyNjUyMGItNTA2Zi00OWY5LWJjZGEtNWU1MWFmYjYwMDM0XkEyXkFqcGdeQXVyNTg5NTY2MTQ@._V1_Ratio1.3182_AL_.jpg",
            "name": "Bill Smitrovich",
            "asCharacter": "General Gabriel"
          },
          {
            "id": "nm0046223",
            "image": "https://imdb-api.com/images/original/MV5BYTc1NDdhMmYtNWY1Ni00MTg5LTk0ZjMtZWUzMDZjZWU1ODFmXkEyXkFqcGdeQXVyMjUzNTM0Ng@@._V1_Ratio0.7273_AL_.jpg",
            "name": "Sayed Badreya",
            "asCharacter": "Abu Bakaar"
          },
          {
            "id": "nm0079273",
            "image": "https://imdb-api.com/images/original/MV5BNjUzMDIzNjkxMl5BMl5BanBnXkFtZTgwNjgyNzA0MjI@._V1_Ratio1.5000_AL_.jpg",
            "name": "Paul Bettany",
            "asCharacter": "JARVIS (voice)"
          },
          {
            "id": "nm0269463",
            "image": "https://imdb-api.com/images/original/MV5BNjcwNzg4MjktNDNlMC00M2U1LWJmMjgtZTVkMmI4MDI2MTVmXkEyXkFqcGdeQXVyMjI4MDI0NTM@._V1_Ratio0.7273_AL_.jpg",
            "name": "Jon Favreau",
            "asCharacter": "Hogan"
          },
          {
            "id": "nm0082526",
            "image": "https://imdb-api.com/images/original/MV5BMTA4NDg5MTc4NzNeQTJeQWpwZ15BbWU3MDEyMDIwNjE@._V1_Ratio0.7273_AL_.jpg",
            "name": "Peter Billingsley",
            "asCharacter": "William Ginter Riva"
          },
          {
            "id": "nm0347375",
            "image": "https://imdb-api.com/images/original/MV5BMTkzMzUzMzMwNF5BMl5BanBnXkFtZTgwNjk3MTk2OTE@._V1_Ratio1.5000_AL_.jpg",
            "name": "Tim Guinee",
            "asCharacter": "Major Allen"
          },
          {
            "id": "nm0528164",
            "image": "https://imdb-api.com/images/original/MV5BOTQ1MjE3NDM1OF5BMl5BanBnXkFtZTcwNjg1NDIzMQ@@._V1_Ratio0.8182_AL_.jpg",
            "name": "Will Lyman",
            "asCharacter": "Award Ceremony Narrator (voice)"
          }
        ],
        "fullCast": null,
        "genres": "Action, Adventure, Sci-Fi",
        "genreList": [
          {
            "key": "Action",
            "value": "Action"
          },
          {
            "key": "Adventure",
            "value": "Adventure"
          },
          {
            "key": "Sci-Fi",
            "value": "Sci-Fi"
          }
        ],
        "companies": "Paramount Pictures, Marvel Enterprises, Marvel Studios",
        "companyList": [
          {
            "id": "co0023400",
            "name": "Paramount Pictures"
          },
          {
            "id": "co0095134",
            "name": "Marvel Enterprises"
          },
          {
            "id": "co0051941",
            "name": "Marvel Studios"
          }
        ],
        "countries": "USA, Canada",
        "countryList": [
          {
            "key": "USA",
            "value": "USA"
          },
          {
            "key": "Canada",
            "value": "Canada"
          }
        ],
        "languages": "English, Persian, Urdu, Arabic, Kurdish, Hindi, Hungarian",
        "languageList": [
          {
            "key": "English",
            "value": "English"
          },
          {
            "key": "Persian",
            "value": "Persian"
          },
          {
            "key": "Urdu",
            "value": "Urdu"
          },
          {
            "key": "Arabic",
            "value": "Arabic"
          },
          {
            "key": "Kurdish",
            "value": "Kurdish"
          },
          {
            "key": "Hindi",
            "value": "Hindi"
          },
          {
            "key": "Hungarian",
            "value": "Hungarian"
          }
        ],
        "contentRating": "PG-13",
        "imDbRating": "7.9",
        "imDbRatingVotes": "938380",
        "metacriticRating": "79",
        "ratings": {
          "imDbId": "tt0371746",
          "title": "Iron Man",
          "fullTitle": "Iron Man (2008)",
          "type": "Movie",
          "year": "2008",
          "imDb": "7.9",
          "metacritic": "79",
          "theMovieDb": "7.6",
          "rottenTomatoes": "94",
          "tV_com": "8.9",
          "filmAffinity": "6.6",
          "errorMessage": ""
        },
        "wikipedia": null,
        "posters": null,
        "images": null,
        "trailer": {
          "imDbId": "tt0371746",
          "title": "Iron Man",
          "fullTitle": "Iron Man (2008)",
          "type": "Movie",
          "year": "2008",
          "videoId": "vi447873305",
          "videoTitle": "Iron Man",
          "videoDescription": "Iron Man trailer",
          "thumbnailUrl": "https://m.media-amazon.com/images/M/MV5BMjEyNzQ0MjE2OF5BMl5BanBnXkFtZTcwMTkyNjE5Ng@@._V1_.jpg",
          "uploadDate": "11/21/2007 18:02:03",
          "link": "https://www.imdb.com/video/vi447873305",
          "linkEmbed": "https://www.imdb.com/video/imdb/vi447873305/imdb/embed",
          "errorMessage": ""
        },
        "boxOffice": {
          "budget": "$140,000,000 (estimated)",
          "openingWeekendUSA": "$98,618,668, 4 May 2008",
          "grossUSA": "$319,034,126",
          "cumulativeWorldwideGross": "$585,796,247"
        },
        "tagline": "Get ready for a different breed of heavy metal hero.",
        "keywords": "billionaire,inventor,robot suit,based on comic,marvel cinematic universe",
        "keywordList": [
          "billionaire",
          "inventor",
          "robot suit",
          "based on comic",
          "marvel cinematic universe"
        ],
        "similars": [
          {
            "id": "tt1228705",
            "title": "Iron Man 2",
            "fullTitle": "Iron Man 2 (2010)",
            "year": "2010",
            "image": "https://imdb-api.com/images/original/MV5BMTM0MDgwNjMyMl5BMl5BanBnXkFtZTcwNTg3NzAzMw@@._V1_Ratio0.6737_AL_.jpg",
            "plot": "With the world now aware of his identity as Iron Man, Tony Stark must contend with both his declining health and a vengeful mad man with ties to his father's legacy.",
            "directors": "Jon Favreau",
            "stars": "Robert Downey Jr., Mickey Rourke, Gwyneth Paltrow",
            "genres": "Action, Adventure, Sci-Fi",
            "imDbRating": "7"
          },
          {
            "id": "tt1300854",
            "title": "Iron Man 3",
            "fullTitle": "Iron Man 3 (2013)",
            "year": "2013",
            "image": "https://imdb-api.com/images/original/MV5BMjE5MzcyNjk1M15BMl5BanBnXkFtZTcwMjQ4MjcxOQ@@._V1_Ratio0.6947_AL_.jpg",
            "plot": "When Tony Stark's world is torn apart by a formidable terrorist called the Mandarin, he starts an odyssey of rebuilding and retribution.",
            "directors": "Shane Black",
            "stars": "Robert Downey Jr., Guy Pearce, Gwyneth Paltrow",
            "genres": "Action, Adventure, Sci-Fi",
            "imDbRating": "7.1"
          },
          {
            "id": "tt0458339",
            "title": "Captain America: The First Avenger",
            "fullTitle": "Captain America: The First Avenger (2011)",
            "year": "2011",
            "image": "https://imdb-api.com/images/original/MV5BMTYzOTc2NzU3N15BMl5BanBnXkFtZTcwNjY3MDE3NQ@@._V1_Ratio0.6737_AL_.jpg",
            "plot": "Steve Rogers, a rejected military soldier, transforms into Captain America after taking a dose of a \"Super-Soldier serum\". But being Captain America comes at a price as he attempts to take down a war monger and a terrorist organization.",
            "directors": "Joe Johnston",
            "stars": "Chris Evans, Hugo Weaving, Samuel L. Jackson",
            "genres": "Action, Adventure, Sci-Fi",
            "imDbRating": "6.9"
          },
          {
            "id": "tt0800369",
            "title": "Thor",
            "fullTitle": "Thor (2011)",
            "year": "2011",
            "image": "https://imdb-api.com/images/original/MV5BOGE4NzU1YTAtNzA3Mi00ZTA2LTg2YmYtMDJmMThiMjlkYjg2XkEyXkFqcGdeQXVyNTgzMDMzMTg@._V1_Ratio0.6737_AL_.jpg",
            "plot": "The powerful but arrogant god Thor is cast out of Asgard to live amongst humans in Midgard (Earth), where he soon becomes one of their finest defenders.",
            "directors": "Kenneth Branagh",
            "stars": "Chris Hemsworth, Anthony Hopkins, Natalie Portman",
            "genres": "Action, Adventure, Fantasy",
            "imDbRating": "7"
          },
          {
            "id": "tt0848228",
            "title": "The Avengers",
            "fullTitle": "The Avengers (2012)",
            "year": "2012",
            "image": "https://imdb-api.com/images/original/MV5BNDYxNjQyMjAtNTdiOS00NGYwLWFmNTAtNThmYjU5ZGI2YTI1XkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_Ratio0.6737_AL_.jpg",
            "plot": "Earth's mightiest heroes must come together and learn to fight as a team if they are going to stop the mischievous Loki and his alien army from enslaving humanity.",
            "directors": "Joss Whedon",
            "stars": "Robert Downey Jr., Chris Evans, Scarlett Johansson",
            "genres": "Action, Adventure, Sci-Fi",
            "imDbRating": "8"
          },
          {
            "id": "tt1843866",
            "title": "Captain America: The Winter Soldier",
            "fullTitle": "Captain America: The Winter Soldier (2014)",
            "year": "2014",
            "image": "https://imdb-api.com/images/original/MV5BMzA2NDkwODAwM15BMl5BanBnXkFtZTgwODk5MTgzMTE@._V1_Ratio0.6842_AL_.jpg",
            "plot": "As Steve Rogers struggles to embrace his role in the modern world, he teams up with a fellow Avenger and S.H.I.E.L.D agent, Black Widow, to battle a new threat from history: an assassin known as the Winter Soldier.",
            "directors": "Directors: Anthony Russo, Joe Russo",
            "stars": "Chris Evans, Samuel L. Jackson, Scarlett Johansson",
            "genres": "Action, Adventure, Sci-Fi",
            "imDbRating": "7.7"
          },
          {
            "id": "tt2395427",
            "title": "Avengers: Age of Ultron",
            "fullTitle": "Avengers: Age of Ultron (2015)",
            "year": "2015",
            "image": "https://imdb-api.com/images/original/MV5BMTM4OGJmNWMtOTM4Ni00NTE3LTg3MDItZmQxYjc4N2JhNmUxXkEyXkFqcGdeQXVyNTgzMDMzMTg@._V1_Ratio0.6737_AL_.jpg",
            "plot": "When Tony Stark and Bruce Banner try to jump-start a dormant peacekeeping program called Ultron, things go horribly wrong and it's up to Earth's mightiest heroes to stop the villainous Ultron from enacting his terrible plan.",
            "directors": "Joss Whedon",
            "stars": "Robert Downey Jr., Chris Evans, Mark Ruffalo",
            "genres": "Action, Adventure, Sci-Fi",
            "imDbRating": "7.3"
          },
          {
            "id": "tt3498820",
            "title": "Captain America: Civil War",
            "fullTitle": "Captain America: Civil War (2016)",
            "year": "2016",
            "image": "https://imdb-api.com/images/original/MV5BMjQ0MTgyNjAxMV5BMl5BanBnXkFtZTgwNjUzMDkyODE@._V1_Ratio0.6737_AL_.jpg",
            "plot": "Political involvement in the Avengers' affairs causes a rift between Captain America and Iron Man.",
            "directors": "Directors: Anthony Russo, Joe Russo",
            "stars": "Chris Evans, Robert Downey Jr., Scarlett Johansson",
            "genres": "Action, Adventure, Sci-Fi",
            "imDbRating": "7.8"
          },
          {
            "id": "tt1981115",
            "title": "Thor: The Dark World",
            "fullTitle": "Thor: The Dark World (2013)",
            "year": "2013",
            "image": "https://imdb-api.com/images/original/MV5BMTQyNzAwOTUxOF5BMl5BanBnXkFtZTcwMTE0OTc5OQ@@._V1_Ratio0.6947_AL_.jpg",
            "plot": "When the Dark Elves attempt to plunge the universe into darkness, Thor must embark on a perilous and personal journey that will reunite him with doctor Jane Foster.",
            "directors": "Alan Taylor",
            "stars": "Chris Hemsworth, Natalie Portman, Tom Hiddleston",
            "genres": "Action, Adventure, Fantasy",
            "imDbRating": "6.9"
          },
          {
            "id": "tt0478970",
            "title": "Ant-Man",
            "fullTitle": "Ant-Man (2015)",
            "year": "2015",
            "image": "https://imdb-api.com/images/original/MV5BMjM2NTQ5Mzc2M15BMl5BanBnXkFtZTgwNTcxMDI2NTE@._V1_Ratio0.6737_AL_.jpg",
            "plot": "Armed with a super-suit with the astonishing ability to shrink in scale but increase in strength, cat burglar Scott Lang must embrace his inner hero and help his mentor, Dr. Hank Pym, plan and pull off a heist that will save the world.",
            "directors": "Peyton Reed",
            "stars": "Paul Rudd, Michael Douglas, Corey Stoll",
            "genres": "Action, Adventure, Comedy",
            "imDbRating": "7.3"
          },
          {
            "id": "tt1211837",
            "title": "Doctor Strange",
            "fullTitle": "Doctor Strange (2016)",
            "year": "2016",
            "image": "https://imdb-api.com/images/original/MV5BNjgwNzAzNjk1Nl5BMl5BanBnXkFtZTgwMzQ2NjI1OTE@._V1_Ratio0.6737_AL_.jpg",
            "plot": "While on a journey of physical and spiritual healing, a brilliant neurosurgeon is drawn into the world of the mystic arts.",
            "directors": "Scott Derrickson",
            "stars": "Benedict Cumberbatch, Chiwetel Ejiofor, Rachel McAdams",
            "genres": "Action, Adventure, Fantasy",
            "imDbRating": "7.5"
          },
          {
            "id": "tt2015381",
            "title": "Guardians of the Galaxy",
            "fullTitle": "Guardians of the Galaxy (2014)",
            "year": "2014",
            "image": "https://imdb-api.com/images/original/MV5BMTAwMjU5OTgxNjZeQTJeQWpwZ15BbWU4MDUxNDYxODEx._V1_Ratio0.6737_AL_.jpg",
            "plot": "A group of intergalactic criminals must pull together to stop a fanatical warrior with plans to purge the universe.",
            "directors": "James Gunn",
            "stars": "Chris Pratt, Vin Diesel, Bradley Cooper",
            "genres": "Action, Adventure, Comedy",
            "imDbRating": "8"
          }
        ],
        "tvSeriesInfo": null,
        "tvEpisodeInfo": null,
        "errorMessage": ""
    },
    "youtubeData": {
      "imDbId": "tt0371746",
      "title": "Iron Man",
      "fullTitle": "Iron Man (2008)",
      "type": "Movie",
      "year": "2008",
      "videoId": "8hYlB38asDY",
      "videoUrl": "https://www.youtube.com/watch?v=8hYlB38asDY",
      "errorMessage": ""
    }
  }