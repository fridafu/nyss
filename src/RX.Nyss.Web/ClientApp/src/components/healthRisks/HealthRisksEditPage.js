import React, { useEffect, useState, Fragment } from 'react';
import { connect } from "react-redux";
import { useLayout } from '../../utils/layout';
import { validators, createForm, useCustomErrors } from '../../utils/forms';
import * as healthRisksActions from './logic/healthRisksActions';
import Layout from '../layout/Layout';
import Form from '../forms/form/Form';
import FormActions from '../forms/formActions/FormActions';
import SubmitButton from '../forms/submitButton/SubmitButton';
import Typography from '@material-ui/core/Typography';
import TextInputField from '../forms/TextInputField';
import Button from "@material-ui/core/Button";
import { Loading } from '../common/loading/Loading';
import { useMount } from '../../utils/lifecycle';
import Grid from '@material-ui/core/Grid';
import SelectField from '../forms/SelectField';
import MenuItem from "@material-ui/core/MenuItem";
import { healthRiskTypes } from './logic/healthRisksConstants';
import { getSaveFormModel } from './logic/healthRisksService';
import { strings, stringKeys, stringsFormat } from '../../strings';
import { ValidationMessage } from '../forms/ValidationMessage';

const HealthRisksEditPageComponent = (props) => {
  const [form, setForm] = useState(null);
  const [reportCountThreshold, setReportCountThreshold] = useState(null);

  useMount(() => {
    props.openEdition(props.match);
  });

  useEffect(() => {
    if (form && reportCountThreshold <= 1){
      form.fields.alertRuleDaysThreshold.update("");
      form.fields.alertRuleKilometersThreshold.update("");
    }
    return;
  }, [form, reportCountThreshold])

  useEffect(() => {
    if (!props.data) {
      return;
    }
    setReportCountThreshold(props.data.alertRuleCountThreshold);
    let fields = {
      healthRiskCode: props.data.healthRiskCode.toString(),
      healthRiskType: props.data.healthRiskType,
      alertRuleCountThreshold: props.data.alertRuleCountThreshold,
      alertRuleDaysThreshold: props.data.alertRuleDaysThreshold,
      alertRuleKilometersThreshold: props.data.alertRuleKilometersThreshold
    };

    let validation = {
      healthRiskCode: [validators.required, validators.integer],
      healthRiskType: [validators.required],
      alertRuleCountThreshold: [validators.integer],
      alertRuleDaysThreshold: [
        validators.requiredWhen(f => f.alertRuleCountThreshold > 1),
        validators.inRange(1, 365)
      ],
      alertRuleKilometersThreshold: [
        validators.requiredWhen(f => f.alertRuleCountThreshold > 1),
        validators.inRange(1, 9999)
      ]
    };

    const finalFormData = props.contentLanguages
      .map(lang => ({ lang, content: props.data.languageContent.find(lc => lc.languageId === lang.id) }))
      .reduce((result, { lang, content }) => ({
        fields: {
          ...result.fields,
          [`contentLanguage_${lang.id}_name`]: content && content.name,
          [`contentLanguage_${lang.id}_caseDefinition`]: content && content.caseDefinition,
          [`contentLanguage_${lang.id}_feedbackMessage`]: content && content.feedbackMessage
        },
        validation: {
            ...result.validation,
            [`contentLanguage_${lang.id}_name`]: [validators.required, validators.maxLength(100)],
            [`contentLanguage_${lang.id}_caseDefinition`]: [validators.required, validators.maxLength(500)],
            [`contentLanguage_${lang.id}_feedbackMessage`]: [validators.required, validators.maxLength(160)]
          }
      }), { fields, validation });

    const newForm = createForm(finalFormData.fields, finalFormData.validation);
    setForm(newForm);
    newForm.fields.alertRuleCountThreshold.subscribe(({ newValue }) => setReportCountThreshold(newValue));
  }, [props.data, props.contentLanguages]);

  const [healthRiskTypesData] = useState(healthRiskTypes.map(t => ({
    value: t,
    label: strings(stringKeys.healthRisk.constants.healthRiskType[t.toLowerCase()])
  })));

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!form.isValid()) {
      return;
    };

    props.edit(props.data.id, getSaveFormModel(form.getValues(), props.contentLanguages));
  };

  useCustomErrors(form, props.formError);

  if (props.isFetching || !form) {
    return <Loading />;
  }

  return (
    <Fragment>
      {props.formError && <ValidationMessage message={props.formError.message} />}

      <Form onSubmit={handleSubmit} fullWidth style={{ maxWidth: 800 }}>
        <Grid container spacing={2}>
          <Grid item xs={3}>
            <TextInputField
              label={strings(stringKeys.healthRisk.form.healthRiskCode)}
              name="healthRiskCode"
              field={form.fields.healthRiskCode}
            />
          </Grid>
          <Grid item xs={9}>
            <SelectField
              label={strings(stringKeys.healthRisk.form.healthRiskType)}
              name="healthRiskType"
              field={form.fields.healthRiskType}
            >
              {healthRiskTypesData.map(({ value, label }) => (
                <MenuItem key={`healthRiskType${value}`} value={value}>{label}</MenuItem>
              ))}
            </SelectField>
          </Grid>

          {props.contentLanguages.map(lang => (
            <Fragment key={`contentLanguage${lang.id}`}>
              <Grid item xs={12}>
                <Typography variant="h3">{stringsFormat(stringKeys.healthRisk.form.translationsSetion, { language: lang.name })}</Typography>

                <Grid container spacing={2}>
                  <Grid item xs={12}>
                    <TextInputField
                      label={strings(stringKeys.healthRisk.form.contentLanguageName)}
                      name={`contentLanguage_${lang.id}_name`}
                      field={form.fields[`contentLanguage_${lang.id}_name`]}
                    />
                  </Grid>
                  <Grid item xs={6}>
                    <TextInputField
                      label={strings(stringKeys.healthRisk.form.contentLanguageCaseDefinition)}
                      name={`contentLanguage_${lang.id}_caseDefinition`}
                      field={form.fields[`contentLanguage_${lang.id}_caseDefinition`]}
                      multiline
                    />
                  </Grid>
                  <Grid item xs={6}>
                    <TextInputField
                      label={strings(stringKeys.healthRisk.form.contentLanguageFeedbackMessage)}
                      name={`contentLanguage_${lang.id}_feedbackMessage`}
                      field={form.fields[`contentLanguage_${lang.id}_feedbackMessage`]}
                      multiline
                    />
                  </Grid>
                </Grid>
              </Grid>
            </Fragment>
          ))}

          <Grid item xs={12}>
            <Typography variant="h3">{strings(stringKeys.healthRisk.form.alertsSetion)}</Typography>
            <Typography variant="subtitle1">{strings(stringKeys.healthRisk.form.alertRuleDescription)}</Typography>
          </Grid>

          <Grid item xs={4}>
            <TextInputField
              label={strings(stringKeys.healthRisk.form.alertRuleCountThreshold)}
              name="alertRuleCountThreshold"
              field={form.fields.alertRuleCountThreshold}
            />
          </Grid>

          <Grid item xs={4}>
            <TextInputField
              label={strings(stringKeys.healthRisk.form.alertRuleDaysThreshold)}
              name="alertRuleDaysThreshold"
              field={form.fields.alertRuleDaysThreshold}
              disabled={!reportCountThreshold || reportCountThreshold <= 1}
            />
          </Grid>

          <Grid item xs={4}>
            <TextInputField
              label={strings(stringKeys.healthRisk.form.alertRuleKilometersThreshold)}
              name="alertRuleKilometersThreshold"
              field={form.fields.alertRuleKilometersThreshold}
              disabled={!reportCountThreshold || reportCountThreshold <= 1}
            />
          </Grid>
        </Grid>

        <FormActions>
          <Button onClick={() => props.goToList()}>{strings(stringKeys.form.cancel)}</Button>
          <SubmitButton isFetching={props.isSaving}>{strings(stringKeys.healthRisk.form.update)}</SubmitButton>
        </FormActions>
      </Form>
    </Fragment>
  );
}

HealthRisksEditPageComponent.propTypes = {
};

const mapStateToProps = state => ({
  contentLanguages: state.appData.contentLanguages,
  isFetching: state.healthRisks.formFetching,
  isSaving: state.healthRisks.formSaving,
  formError: state.healthRisks.formError,
  data: state.healthRisks.formData
});

const mapDispatchToProps = {
  openEdition: healthRisksActions.openEdition.invoke,
  goToList: healthRisksActions.goToList,
  edit: healthRisksActions.edit.invoke
};

export const HealthRisksEditPage = useLayout(
  Layout,
  connect(mapStateToProps, mapDispatchToProps)(HealthRisksEditPageComponent)
);
