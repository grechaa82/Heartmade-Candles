import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import MainInfoCandles, { CandleData } from '../modules/MainInfoCandles';

type CandleDetailsParams = {
    id: string;
};

const CandleDetailsPage: React.FC = () => {
    const { id } = useParams<CandleDetailsParams>();
    const [candleData, setcandleData] = useState<any>();

    useEffect(() => {
        async function fetchCandle() {
            const response = await fetch(`http://localhost:5000/api/admin/candles/${id}`);
            const data = await response.json();
            setcandleData(data);
        }
        fetchCandle();
        }, [id]);

  return (
    <div className="candles">
        {candleData ? <MainInfoCandles candleData={candleData} /> : null}
    </div>
  );
};

export default CandleDetailsPage;
