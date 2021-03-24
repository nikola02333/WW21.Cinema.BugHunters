import React from 'react'

export const getRoundedRating = (rating: number) => {
    const result = Math.round(rating);
    return <span className="float-right" >Rating: {result}/10</span>;
  };