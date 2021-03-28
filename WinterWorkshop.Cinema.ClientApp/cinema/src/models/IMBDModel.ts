export interface IMDB {
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
    genres:           Genres;
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

export enum Genres {
    ActionAdventureComedy = "Action, Adventure, Comedy",
    ActionAdventureFantasy = "Action, Adventure, Fantasy",
    ActionAdventureSciFi = "Action, Adventure, Sci-Fi",
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
    genres:     Genres;
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
