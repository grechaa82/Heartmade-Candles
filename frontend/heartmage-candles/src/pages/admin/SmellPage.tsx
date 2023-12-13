import { FC, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

import MainInfoSmell from '../../modules/admin/MainInfoSmell';
import { Smell } from '../../types/Smell';
import { SmellRequest } from '../../types/Requests/SmellRequest';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';

import { SmellsApi } from '../../services/SmellsApi';

import Style from './SmellPage.module.css';

type SmellParams = {
  id: string;
};

const SmellPage: FC = () => {
  const { id } = useParams<SmellParams>();
  const [smellData, setSmellData] = useState<Smell>();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const handleChangesSmell = (updatedSmell: Smell) => {
    setSmellData((prevSmellData) => ({
      ...prevSmellData,
      ...updatedSmell,
    }));
  };

  const updateSmell = async (updatedItem: Smell) => {
    if (id) {
      const smellRequest: SmellRequest = {
        title: updatedItem.title,
        description: updatedItem.description,
        price: updatedItem.price,
        isActive: updatedItem.isActive,
      };

      const updatedSmellResponse = await SmellsApi.update(id, smellRequest);
      if (updatedSmellResponse.error) {
        setErrorMessage([
          ...errorMessage,
          updatedSmellResponse.error as string,
        ]);
      }
    }
  };

  useEffect(() => {
    async function fetchSmell() {
      if (id) {
        const smellResponse = await SmellsApi.getById(id);
        if (smellResponse.data && !smellResponse.error) {
          setSmellData(smellResponse.data);
        } else {
          setErrorMessage([...errorMessage, smellResponse.error as string]);
        }
      }
    }

    fetchSmell();
  }, [id]);

  return (
    <>
      <div className="smells">
        {smellData && (
          <MainInfoSmell
            data={smellData}
            handleChangesSmell={handleChangesSmell}
            onSave={updateSmell}
          />
        )}
      </div>
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default SmellPage;
