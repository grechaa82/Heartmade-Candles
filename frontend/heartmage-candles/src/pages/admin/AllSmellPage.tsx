import { FC, useState } from 'react';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import CreateSmellPopUp from '../../modules/admin/PopUp/Smell/CreateSmellPopUp';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useSmellsQuery from '../../hooks/useSmellsQuery';

import Style from './AllSmellPage.module.css';

export interface AllSmellPageProps {}

const AllSmellPage: FC<AllSmellPageProps> = () => {
  const { data, isLoading, createSmell, deleteSmell, updateIsActiveSmell } =
    useSmellsQuery();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  if (isLoading) {
    return <div>...Loading</div>;
  }

  return (
    <>
      {isLoading}
      <ProductsGrid
        data={data}
        title="Запахи"
        pageUrl="smells"
        renderPopUpComponent={(onClose) => (
          <CreateSmellPopUp
            onClose={onClose}
            title="Создать запах"
            onSave={createSmell}
          />
        )}
        deleteProduct={deleteSmell}
        updateIsActiveProduct={updateIsActiveSmell}
      />
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default AllSmellPage;
