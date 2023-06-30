import { FC, useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import MainInfoWick from "../modules/MainInfoWick";
import { Wick } from "../types/Wick";
import { WickRequest } from "../types/Requests/WickRequest";

import { getWickById, putWick } from "../Api";

type WickParams = {
  id: string;
};

const WickPage: FC = () => {
  const { id } = useParams<WickParams>();
  const [wickData, setWickData] = useState<Wick>();

  const handleChangesWick = (updatedWick: Wick) => {
    setWickData((prevWickData) => ({
      ...prevWickData,
      ...updatedWick,
    }));
  };

  const updateWick = (updatedItem: Wick) => {
    if (id) {
      const wickRequest: WickRequest = {
        title: updatedItem.title,
        description: updatedItem.description,
        price: updatedItem.price,
        imageURL: updatedItem.imageURL,
        isActive: updatedItem.isActive,
      };
      putWick(id, wickRequest);
    }
  };

  useEffect(() => {
    async function fetchWick() {
      if (id) {
        const data = await getWickById(id);
        setWickData(data);
      }
    }

    fetchWick();
  }, [id]);

  return (
    <>
      <div className="wicks">
        {wickData && (
          <MainInfoWick
            data={wickData}
            handleChangesWick={handleChangesWick}
            onSave={updateWick}
          />
        )}
      </div>
    </>
  );
};

export default WickPage;
