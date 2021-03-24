namespace WinterWorkShop.Cinema.Domain.Common
{
    public static class Messages
    {
        
        #region Payments
        public const string PAYMENT_CREATION_ERROR = "Connection error, occured while creating new payment, please try again";
        #endregion

        #region Auditoriums
        public const string AUDITORIUM_GET_ALL_AUDITORIUMS_ERROR = "Error occured while getting all auditoriums, please try again.";
        public const string AUDITORIUM_PROPERTIE_NAME_NOT_VALID = "The auditorium Name cannot be longer than 50 characters.";
        public const string AUDITORIUM_PROPERTIE_SEATROWSNUMBER_NOT_VALID = "The auditorium number of seats rows must be between 1-20.";
        public const string AUDITORIUM_PROPERTIE_SEATNUMBER_NOT_VALID = "The auditorium number of seats number must be between 1-20.";
        public const string AUDITORIUM_CREATION_ERROR = "Error occured while creating new auditorium, please try again.";
        public const string AUDITORIUM_SEATS_CREATION_ERROR = "Error occured while creating seats for auditorium, please try again.";
        public const string AUDITORIUM_SAME_NAME = "Cannot create new auditorium, auditorium with same name alredy exist.";
        public const string AUDITORIUM_UNVALID_CINEMAID = "Cannot create new auditorium, auditorium with given cinemaId does not exist.";
        public const string AUDITORIUM_GET_BY_ID_ERROR = "Auditorium doesn't exist.";
        public const string AUDITORIUM_UPDATE_CINEMAID = "Cinema doesn't exist.";
        public const string AUDITORIUM_UPDATE_ERROR = "Auditorium doesn't exist.";
        public const string AUDITORIUM_NO_NAME = "Auditorium with this name doesn't exist";
        public const string AUDITORIUM_ID_NULL = "Error occured while getting user by Id, please try again.";
        public const string AUDITORIUM_NOT_IN_CINEMA = "Auditorium does not exist in given cinema";

        #endregion

        #region Cinemas
        public const string CINEMA_GET_ALL_CINEMAS_ERROR = "Error occured while getting all cinemas, please try again";
        public const string CINEMA_ID_NOT_FOUND = "Error occured while getting cinema by id";
        public const string CINEMA_PROPERTIE_NAME_REQUIERED = "The cinema name cannot be longer than 50 characters.";
        public const string CINEMA_PROPERTIE_CITY_NAME_REQUIERED = "The cinema city cannot be longer than 50 characters.";
        public const string CINEMA_PROPERTIE_ADRESS_REQUIERED = "The cinema adress cannot be longer than 50 characters.";
        #endregion

        #region Movies        
        public const string MOVIE_DOES_NOT_EXIST = "Movie does not exist.";
        public const string MOVIE_PROPERTIE_TITLE_NOT_VALID = "The movie title cannot be longer than 50 characters.";
        public const string MOVIE_PROPERTIE_YEAR_NOT_VALID = "The movie year must be between 1895-2100.";
        public const string MOVIE_PROPERTIE_RATING_NOT_VALID = "The movie rating must be between 1-10.";
        public const string MOVIE_CREATION_ERROR = "Error occured while creating new movie, please try again.";
        public const string MOVIE_GET_ALL_CURRENT_MOVIES_ERROR = "Error occured while getting current movies, please try again.";
        public const string MOVIE_GET_BY_ID = "Error occured while getting movie by Id, please try again.";
        public const string MOVIE_GET_ALL_MOVIES_ERROR = "Error occured while getting all movies, please try again.";
        public const string MOVIE_DELETE_ERROR = "Error occured while deleting  movie.";
        public const string MOVIE_UPDATE_ERROR = "Error occured while updating  movie.";
        public const string MOVIE_ACTIVATE_DEACTIVATE_ERROR = "Error occured while activate-deactivate  movie.";
        public const string MOVIE_NOT_IN_AUDITORIUM = "Movie is not showing in given auditorium";
        public const string MOVIE_SEARCH_BY_TAG_NOT_FOUND = "Movie search by tag not found";
        #endregion

        #region Projections
        public const string PROJECTION_GET_ALL_PROJECTIONS_ERROR = "Error occured while getting all projections, please try again.";
        public const string PROJECTION_CREATION_ERROR = "Error occured while creating new projection, please try again.";
        public const string PROJECTIONS_AT_SAME_TIME = "Cannot create new projection, there are projections at same time alredy.";
        public const string PROJECTION_IN_PAST = "Projection time cannot be in past.";
        public const string PROJECTION_GET_BY_ID = "Error occured while getting projection by Id, please try again.";
        #endregion

        #region Seats
        public const string SEATS_IN_AUDITORIUM = "There are no seats in given auditorium.";
        public const string SEAT_GET_ALL_SEATS_ERROR = "Error occured while getting all seats, please try again.";
        public const string SEAT_GET_BY_ID = "Error occured while getting seat by Id, please try again.";
        public const string SEAT_NOT_IN_AUDITORIUM_OF_PROJECTION = "Error occured becouse Id of seat is not in the auditorium for the given projection";
        public const string SEAT_RESERVED = "Seat is reservet for selected projection";
        public const string SEAT_ID_NULL = "Error occured becouse Id field is empty or null.";
        #endregion

        #region Users
        public const string USER_NOT_FOUND = "User does not exist.";
        public const string USER_NAME_REQUIRED = "User require name.";
        public const string USER_USERNAME_REQUIRED = "User require username.";
        public const string USER_LASTNAME_REQUIRED = "User require lastname.";
        public const string USER_ID_NULL = "Error occured while getting user by Id, please try again.";
        public const string USER_CREATION_ERROR = "Error occured while creating new user, please try again.";
        public const string USER_CREATION_ERROR_USERNAME_EXISTS = "Error occured while creating new user, username exists.";
        public const string USER_INCREMENT_POINTS_ERROR = "Error occured while  incrementing user points.";
        #endregion

        #region Ticket       
        public const string TICKET_GET_BY_ID = "Error occured while getting ticket by Id, please try again.";
        public const string TICKET_ID_NULL = "Error occured becouse Id field is empty or null.";
        public const string TICKET_DOES_NOT_EXIST = "Ticket does not exist.";
        public const string TICKET_CREATION_ERROR = "Error occured while creating new ticket, please try again.";
        public const string TICKET_GET_ALL_TICKETS_ERROR = "Error occured while getting all tickets, please try again.";
        public const string TICKET_DELTE_ERROR = "Error occured while deleting ticket, please try again.";
        #endregion

        #region Tags       
        public const string TAG_DOES_NOT_EXIST = "Error occured while getting tag by Id, please try again.";
        public const string TAG_DELTE_ERROR = "Error occured while deleting tag, please try again.";

        #endregion
    }
}
