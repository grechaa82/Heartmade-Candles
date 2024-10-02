import { FC, useState } from 'react';
import { useParams } from 'react-router-dom';

import MainInfoSmell from '../../modules/admin/MainInfoSmell';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useSmellByIdQuery from '../../hooks/useSmellByIdQuery';

import Style from './SmellPage.module.css';

type SmellParams = {
  id: string;
};

const SmellPage: FC = () => {
  const { id } = useParams<SmellParams>();
  const { data, isLoading, updateSmell } = useSmellByIdQuery(id);
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  if (isLoading) {
    return <div>...Loading</div>;
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
