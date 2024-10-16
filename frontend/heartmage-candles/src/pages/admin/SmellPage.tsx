import { FC, useState, useContext } from 'react';
import { useParams } from 'react-router-dom';

import MainInfoSmell from '../../modules/admin/MainInfoSmell';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useSmellByIdQuery from '../../hooks/admin/useSmellByIdQuery';
import { AuthContext } from '../../contexts/AuthContext';
import MainInfoSkeleton from '../../modules/admin/MainInfoSkeleton';

import Style from './SmellPage.module.css';

type SmellParams = {
  id: string;
};

const SmellPage: FC = () => {
  const { id } = useParams<SmellParams>();
  const { isAuth } = useContext(AuthContext);
  const { data, isLoading, updateSmell } = useSmellByIdQuery(id, isAuth);
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  if (isLoading) {
    return <MainInfoSkeleton />;
  }

  return (
    <>
      <div className="smells">
        {data && <MainInfoSmell data={data} onSave={updateSmell} />}
      </div>
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default SmellPage;
