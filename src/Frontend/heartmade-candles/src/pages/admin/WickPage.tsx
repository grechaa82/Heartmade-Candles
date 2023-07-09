import { FC, useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import MainInfoWick from "../../modules/admin/MainInfoWick";
import { Wick } from "../../types/Wick";
import { WickRequest } from "../../types/Requests/WickRequest";

import { WicksApi } from "../../services/WicksApi";

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

  const updateWick = async (updatedItem: Wick) => {
    if (id) {
      const wickRequest: WickRequest = {
        title: updatedItem.title,
        description: updatedItem.description,
        price: updatedItem.price,
        imageURL: updatedItem.imageURL,
        isActive: updatedItem.isActive,
      };
      await WicksApi.update(id, wickRequest);
    }
  };

  useEffect(() => {
    async function fetchWick() {
      if (id) {
        const data = await WicksApi.getById(id);
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
