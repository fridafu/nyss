import styles from "./ProjectsDashboardNumbers.module.scss";

import React from 'react';
import Grid from '@material-ui/core/Grid';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import CardHeader from '@material-ui/core/CardHeader';
import { Loading } from '../../common/loading/Loading';
import { stringKeys, strings } from '../../../strings';

export const ProjectsDashboardNumbers = ({ isFetching, projectSummary, reportsType }) => {
  if (isFetching || !projectSummary) {
    return <Loading />;
  }

  const renderNumber = (label, value) => (
    <Grid container spacing={3}>
      <Grid item className={styles.numberName}>{label}</Grid>
      <Grid item className={styles.numberValue}>{value}</Grid>
    </Grid>
  );

  return (
    <Grid container spacing={3}>
      <Grid item sm={3} xs={12} className={styles.numberBox}>
        <Card className={styles.card}>
          <CardHeader title={strings(stringKeys.project.dashboard.numbers.totalReportCountTitle)} />
          <CardContent>
            {renderNumber(strings(stringKeys.project.dashboard.numbers.totalReportCount), projectSummary.reportCount)}
          </CardContent>
        </Card>
      </Grid>

      <Grid item sm={3} xs={12} className={styles.numberBox}>
        <Card className={styles.card}>
          <CardHeader title={strings(stringKeys.project.dashboard.dataCollectors)} />
          <CardContent>
            {renderNumber(strings(stringKeys.project.dashboard.activeDataCollectorCount), projectSummary.activeDataCollectorCount)}
            {renderNumber(strings(stringKeys.project.dashboard.inactiveDataCollectorCount), projectSummary.inactiveDataCollectorCount)}
          </CardContent>
        </Card>
      </Grid>

      {reportsType === "dataCollectionPoint" &&
       <Grid item xs={3} className={styles.numberBox}>
        <Card className={styles.card}>
          <CardHeader title={strings(stringKeys.project.dashboard.dataCollectionPoints)} />
          <CardContent>
            {renderNumber(strings(stringKeys.project.dashboard.referredToHospitalCount), projectSummary.dataCollectionPointSummary.referredToHospitalCount)}
            {renderNumber(strings(stringKeys.project.dashboard.fromOtherVillagesCount), projectSummary.dataCollectionPointSummary.fromOtherVillagesCount)}
            {renderNumber(strings(stringKeys.project.dashboard.deathCount), projectSummary.dataCollectionPointSummary.deathCount)}
          </CardContent>
        </Card>
      </Grid>
      }
    </Grid>
  );
}
