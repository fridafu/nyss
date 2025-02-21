import styles from "./AlertsAssessmentActions.module.scss"

import React, { Fragment, useState } from 'react';
import { stringKeys, strings } from "../../../strings";
import SubmitButton from "../../common/buttons/submitButton/SubmitButton";
import FormActions from "../../forms/formActions/FormActions";
import CancelButton from '../../common/buttons/cancelButton/CancelButton';
import { AlertsEscalationDialog } from './AlertsEscalationDialog';
import { assessmentStatus } from '../logic/alertsConstants';
import { AlertsCloseDialog } from "./AlertsCloseDialog";
import { AlertsEscalationWithoutNotificationDialog } from "./AlertsEscalationWithoutNotificationDialog";
import CheckboxField from "../../forms/CheckboxField";
import { validators, createForm } from '../../../utils/forms';

export const AlertsAssessmentActions = ({ projectId, alertId, alertAssessmentStatus, hasAccess, ...props }) => {
  const [escalationDialogOpened, setEscalationDialogOpened] = useState(false);
  const [escalationWithoutNotificationDialogOpened, setEscalationWithoutNotificationDialogOpened] = useState(false);
  const [closeDialogOpened, setCloseDialogOpened] = useState(false);

  const [form] = useState(() => {
    const fields = {
      comments: "",
      escalateWithoutNotification: false
    };
    const validation = { comments: [validators.maxLength(500)] };
    return createForm(fields, validation);
  });

  const handleEscalateAlert = () => {
    if (form.fields.escalateWithoutNotification.value) {
      setEscalationWithoutNotificationDialogOpened(true);
    } else {
      props.fetchRecipients(alertId);
      setEscalationDialogOpened(true);
    }
  }

  return (
    <Fragment>
      <FormActions>
        <CancelButton onClick={() => props.goToList(projectId)}>{strings(stringKeys.form.cancel)}</CancelButton>

        {!props.isPendingAlertState && (
          <Fragment>
            {alertAssessmentStatus === assessmentStatus.toEscalate && hasAccess && (
              <Fragment>
                <AlertsEscalationDialog
                  alertId={alertId}
                  escalateAlert={props.escalateAlert}
                  isEscalating={props.isEscalating}
                  isFetchingRecipients={props.isFetchingRecipients}
                  notificationEmails={props.notificationEmails}
                  notificationPhoneNumbers={props.notificationPhoneNumbers}
                  isOpened={escalationDialogOpened}
                  close={() => setEscalationDialogOpened(false)}
                />

                <AlertsEscalationWithoutNotificationDialog
                  alertId={alertId}
                  escalateAlert={props.escalateAlert}
                  isEscalating={props.isEscalating}
                  isOpened={escalationWithoutNotificationDialogOpened}
                  close={() => setEscalationWithoutNotificationDialogOpened(false)}
                />

                <div className={styles.escalateWithoutNotificationWrapper}>
                  <CheckboxField className={styles.escalateWithoutNotificationCheckbox}
                    name="escalateWithoutNotification"
                    label={strings(stringKeys.alerts.assess.alert.escalateWithoutNotification)}
                    field={form.fields.escalateWithoutNotification}
                  />

                  <SubmitButton onClick={handleEscalateAlert}>
                    {strings(stringKeys.alerts.assess.alert.escalate)}
                  </SubmitButton>
                </div>
              </Fragment>
            )}

            {(alertAssessmentStatus === assessmentStatus.toDismiss || alertAssessmentStatus === assessmentStatus.rejected) && hasAccess && (
              <SubmitButton isFetching={props.isDismissing} onClick={() => props.dismissAlert(alertId)}>
                {strings(stringKeys.alerts.assess.alert.dismiss)}
              </SubmitButton>
            )}

            {alertAssessmentStatus === assessmentStatus.escalated && hasAccess && (
              <Fragment>
                <AlertsCloseDialog
                  alertId={alertId}
                  closeAlert={props.closeAlert}
                  isClosing={props.isClosing}
                  isOpened={closeDialogOpened}
                  close={() => setCloseDialogOpened(false)}
                />
                <SubmitButton onClick={() => setCloseDialogOpened(true)}>
                  {strings(stringKeys.alerts.assess.alert.close)}
                </SubmitButton>
              </Fragment>
            )}
          </Fragment>
        )}
      </FormActions>
    </Fragment>
  );
}
