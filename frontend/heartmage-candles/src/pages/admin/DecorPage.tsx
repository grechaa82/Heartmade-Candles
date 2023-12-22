import { FC, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

import MainInfoDecor from '../../modules/admin/MainInfoDecor';
import { Decor } from '../../types/Decor';
import { DecorRequest } from '../../types/Requests/DecorRequest';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';

import { DecorsApi } from '../../services/DecorsApi';

import Style from './DecorPage.module.css';

type DecorParams = {
  id: string;
};

const DecorPage: FC = () => {
  const { id } = useParams<DecorParams>();
  const [decorData, setDecorData] = useState<Decor>();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const handleChangesDecor = (updatedDecor: Decor) => {
    setDecorData((prevDecorData) => ({
      ...prevDecorData,
      ...updatedDecor,
    }));
  };

  const updateDecor = async (updatedItem: Decor) => {
    if (id) {
      const decorRequest: DecorRequest = {
        title: updatedItem.title,
        description: updatedItem.description,
        price: updatedItem.price,
        images: updatedItem.images,
        isActive: updatedItem.isActive,
      };

      const updatedDecorResponse = await DecorsApi.update(id, decorRequest);
      if (updatedDecorResponse.error) {
        setErrorMessage([
          ...errorMessage,
          updatedDecorResponse.error as string,
        ]);
      }
    }
  };

  useEffect(() => {
    async function fetchDecors() {
      if (id) {
        const decorResponse = await DecorsApi.getById(id);
        if (decorResponse.data && !decorResponse.error) {
          setDecorData(decorResponse.data);
        } else {
          setErrorMessage([...errorMessage, decorResponse.error as string]);
        }
      }
    }

    fetchDecors();
  }, [id]);

  return (
    <>
      <div className="decors">
        {decorData && (
          <MainInfoDecor
            data={decorData}
            onChangesDecor={handleChangesDecor}
            onSave={updateDecor}
          />
        )}
      </div>
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default DecorPage;
