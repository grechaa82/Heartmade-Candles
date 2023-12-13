import { FC, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

import MainInfoWick from '../../modules/admin/MainInfoWick';
import { Wick } from '../../types/Wick';
import { WickRequest } from '../../types/Requests/WickRequest';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';

import { WicksApi } from '../../services/WicksApi';

import Style from './WickPage.module.css';

type WickParams = {
  id: string;
};

const WickPage: FC = () => {
  const { id } = useParams<WickParams>();
  const [wickData, setWickData] = useState<Wick>();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

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
        images: updatedItem.images,
        isActive: updatedItem.isActive,
      };

      const updatedWickResponse = await WicksApi.update(id, wickRequest);
      if (updatedWickResponse.error) {
        setErrorMessage([...errorMessage, updatedWickResponse.error as string]);
      }
    }
  };

  useEffect(() => {
    async function fetchWick() {
      if (id) {
        const wickResponse = await WicksApi.getById(id);
        if (wickResponse.data && !wickResponse.error) {
          setWickData(wickResponse.data);
        } else {
          setErrorMessage([...errorMessage, wickResponse.error as string]);
        }
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
            onChangesWick={handleChangesWick}
            onSave={updateWick}
          />
        )}
      </div>
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default WickPage;
