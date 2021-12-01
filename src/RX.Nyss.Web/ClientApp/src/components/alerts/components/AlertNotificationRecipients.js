import React from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
} from "@material-ui/core";
import { stringKeys, strings } from "../../../strings";
import styles from "./AlertNotificationRecipients.module.scss";

const AlertNotificationRecipients = ({ recipients }) => {
  return (
    <Table className={styles.table}>
      <TableHead>
        <TableRow>
          <TableCell>{strings(stringKeys.alerts.assess.escalatedTo.role)}</TableCell>
          <TableCell>{strings(stringKeys.alerts.assess.escalatedTo.organization)}</TableCell>
          <TableCell>{strings(stringKeys.alerts.assess.escalatedTo.phoneNumber)}</TableCell>
          <TableCell>{strings(stringKeys.alerts.assess.escalatedTo.email)}</TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        {recipients.map(recipient => <TableRow>
          <TableCell>{recipient.role}</TableCell>
          <TableCell>{recipient.organization}</TableCell>
          <TableCell>{recipient.phoneNumber}</TableCell>
          <TableCell>{recipient.email}</TableCell>
        </TableRow>)}
      </TableBody>
    </Table>
  );
};

export default AlertNotificationRecipients;
