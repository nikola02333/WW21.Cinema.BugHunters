import React from 'react'

export const getRoundedRating = (rating: number) => {
    const result = rating;
    return <span className="float-right">Rating: {result}/10</span>;
  };

export const navigateToProjectionDetails = (id: string, movieId: string,history) => {
    history.push(`TicketReservation/${id}/${movieId}`);
  };