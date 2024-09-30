import React, { useState, useEffect } from 'react';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import { Smell } from '../../types/Smell';
import { SmellRequest } from '../../types/Requests/SmellRequest';
import CreateSmellPopUp from '../../modules/admin/PopUp/Smell/CreateSmellPopUp';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';

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
        setErrorMessage([
          ...errorMessage,
          updatedSmellsResponse.error as string,
        ]);
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
        setErrorMessage([
          ...errorMessage,
          updatedSmellsResponse.error as string,
        ]);
      }
    }
  };

  const handleUpdateIsActiveSmell = async (id: string) => {
    const smell = smellsData.find((x) => x.id === parseInt(id));
    const newSmellRequest: SmellRequest = {
      title: smell.title,
      description: smell.description,
      price: smell.price,
      isActive: !smell.isActive,
    };

    const response = await SmellsApi.update(
      smell.id.toString(),
      newSmellRequest,
    );
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    } else {
      const updatedSmellsResponse = await SmellsApi.getAll();
      if (updatedSmellsResponse.data && !updatedSmellsResponse.error) {
        setSmellsData(updatedSmellsResponse.data);
      } else {
        setErrorMessage([
          ...errorMessage,
          updatedSmellsResponse.error as string,
        ]);
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
        renderPopUpComponent={(onClose) => (
          <CreateSmellPopUp
            onClose={onClose}
            title="Создать запах"
            onSave={handleCreateSmell}
          />
        )}
        deleteProduct={handleDeleteSmell}
        updateIsActiveProduct={handleUpdateIsActiveSmell}
      />
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default AllSmellPage;
