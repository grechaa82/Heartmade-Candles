import React, { useState, useEffect } from 'react';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import { Smell } from '../../types/Smell';
import { SmellRequest } from '../../types/Requests/SmellRequest';
import CreateSmellPopUp from '../../components/admin/PopUp/CreateSmellPopUp';
import ListErrorPopUp from '../../modules/constructor/ListErrorPopUp';

import { SmellsApi } from '../../services/SmellsApi';

import Style from './AllSmellPage.module.css';

export interface AllSmellPageProps {}

const AllSmellPage: React.FC<AllSmellPageProps> = () => {
  const [smellsData, setSmellsData] = useState<Smell[]>([]);

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const handleCreateSmell = async (createdItem: Smell) => {
    const smellRequest: SmellRequest = {
      title: createdItem.title,
      description: createdItem.description,
      price: createdItem.price,
      isActive: createdItem.isActive,
    };
    const response = await SmellsApi.create(smellRequest);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedSmellsResponse = await SmellsApi.getAll();
      if (updatedSmellsResponse.data && !updatedSmellsResponse.error) {
        setSmellsData(updatedSmellsResponse.data);
      } else {
        setErrorMessage([...errorMessage, updatedSmellsResponse.error as string]);
      }
    }
  };

  const handleDeleteSmell = async (id: string) => {
    const response = await SmellsApi.delete(id);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedSmellsResponse = await SmellsApi.getAll();
      if (updatedSmellsResponse.data && !updatedSmellsResponse.error) {
        setSmellsData(updatedSmellsResponse.data);
      } else {
        setErrorMessage([...errorMessage, updatedSmellsResponse.error as string]);
      }
    }
  };

  useEffect(() => {
    async function fetchSmells() {
      const smellsResponse = await SmellsApi.getAll();
      if (smellsResponse.data && !smellsResponse.error) {
        setSmellsData(smellsResponse.data);
      } else {
        setErrorMessage([...errorMessage, smellsResponse.error as string]);
      }
    }
    fetchSmells();
  }, []);

  return (
    <>
      <ProductsGrid
        data={smellsData}
        title="Запахи"
        pageUrl="smells"
        popUpComponent={
          <CreateSmellPopUp
            onClose={() => console.log('Popup closed')}
            title="Создать запах"
            onSave={handleCreateSmell}
          />
        }
        deleteProduct={handleDeleteSmell}
      />
      <div className={Style.popUpNotification}>
        <ListErrorPopUp messages={errorMessage} />
      </div>
    </>
  );
};

export default AllSmellPage;
