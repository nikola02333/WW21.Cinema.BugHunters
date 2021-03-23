import React,{memo} from 'react'

const InfoTable = memo((props:{currentReservationSeats,projectionPrice}) =>{
    console.log("InfoTable");
    return(
        <table className="payment-table">
            <thead className="payment-table__head">
                <tr className="payment-table__row">
                <th className="payment-table__cell">Ulaznice</th>
                <th className="payment-table__cell">Cena</th>
                <th className="payment-table__cell">Ukupno</th>
                </tr>
            </thead>
            <tbody className="payment-table__row">
                <tr>
                <td className="payment-table__cell">
                    <span>{props.currentReservationSeats.length}</span>
                </td>
                <td className="payment-table__cell">{props.projectionPrice},00 RSD</td>
                <td className="payment-table__cell">
                    {props.currentReservationSeats.length * props.projectionPrice},00 RSD
                </td>
                </tr>
            </tbody>
        </table>
    );
});
export default InfoTable;