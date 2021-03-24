import React from 'react'

export const getRoundedRating = (rating: number) => {
    const result = Math.round(rating);
    return <span className="float-right">Rating: {result}/10</span>;
  };

export const navigateToProjectionDetails = (id: string, movieId: string, props) => {
    props.history.push(`TicketReservation/${id}/${movieId}`);
  };