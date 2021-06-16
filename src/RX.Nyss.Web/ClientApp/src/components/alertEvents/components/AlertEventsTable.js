import React, {Fragment, useState} from 'react';
import { stringKeys, strings, stringsFormat } from "../../../strings";
import { TableContainer, Table, TableBody, TableCell, TableHead, TableRow } from "@material-ui/core";
import * as dayjs from "dayjs";
import { logType } from "../logic/alertEventsConstants";
import { escalatedOutcomes } from "../../alerts/logic/alertsConstants";
import { AlertEventExpandableText } from "./AlertEventExpandableText";
import {EditAlertEventDialog} from "./EditAlertEventDialog";
import {TableRowActions} from "../../common/tableRowAction/TableRowActions";
import EditIcon from "@material-ui/icons/Edit";
import {TableRowAction} from "../../common/tableRowAction/TableRowAction";
import {accessMap} from "../../../authentication/accessMap";

export const AlertEventsTable = ({ alertId, list, edit, ...props }) => {
  const [editDialogOpened, setEditDialogOpened] = useState(false);
  const [editDialog, setEditDialog] = useState();

  const formatLogType = (row) => {

    if (row.logType != null) {
      if (row.logType === logType.closedAlert && row.metadata.escalatedOutcome !== escalatedOutcomes.other) {
        return `${stringsFormat(stringKeys.alerts.constants.logType[row.logType], row.metadata)} - ${strings(stringKeys.alerts.constants.escalatedOutcomes[row.metadata.escalatedOutcome])}`;
      } else {
        return stringsFormat(stringKeys.alerts.constants.logType[row.logType], row.metadata);
      }
    }
    else {
      return strings(stringKeys.alerts.constants.eventTypes[row.alertEventType])
    }
  };

      return strings(stringKeys.alerts.constants.eventTypes[row.alertEventType])
    if (row.alertEventSubtype !== null) {
      return strings(stringKeys.alerts.constants.eventSubtypes[row.alertEventSubtype])
    }
  };

  const dashIfEmpty = (text, ...args) => {
    return [text || "-", ...args].filter(x => !!x).join(", ");

  };

  const showEditDialog = (alertEventLogId, text) => {
    setEditDialog(
      <EditAlertEventDialog
      close={() => setEditDialogOpened(false)}
      edit={props.edit}
      alertEventLogId={alertEventLogId}
      alertId={alertId}
      text={text}
    />
    )
    setEditDialogOpened(true)
  }

  return (
    <Fragment>
      <TableContainer sticky="true" >
        <Table>

          <TableHead>
            <TableRow>
              <TableCell style={{ width: "10%", minWidth: 150 }}>{strings(stringKeys.alerts.eventLog.list.date)}</TableCell>
              <TableCell style={{ width: "10%", minWidth: 150 }}>{strings(stringKeys.alerts.eventLog.list.userName)}</TableCell>
              <TableCell style={{ width: "20%", minWidth: 200 }}>{strings(stringKeys.alerts.eventLog.list.type)}</TableCell>
              <TableCell style={{ width: "20%", minWidth: 200 }}>{strings(stringKeys.alerts.eventLog.list.subtype)}</TableCell>
              <TableCell style={{ width: "35%", minWidth: 200 }}>{strings(stringKeys.alerts.eventLog.list.comment)}</TableCell>
              <TableCell style={{ width: "5%", minWidth: 50 }}/>
            </TableRow>
          </TableHead>

          <TableBody>
            {!!list && list.map((row, index) => (
              <TableRow key={index} hover >
                <TableCell>{dayjs(row.date).format("YYYY-MM-DD HH:mm")}</TableCell>
                <TableCell>{dashIfEmpty(row.loggedBy)}</TableCell>
                <TableCell>{formatLogType(row)}</TableCell>
                <TableCell>{formatSubtype(row)}</TableCell>
                <TableCell>
                  <AlertEventExpandableText text={row.text} maxLength={70}/>
                </TableCell>
                <TableCell>
                  {row.alertEventType &&
                  <TableRowActions>
                    <TableRowAction
                      onClick={() => showEditDialog(row.alertEventLogId, row.text) }
                      roles={accessMap.alertEvents.edit}
                      icon={<EditIcon />}
                      title={"Edit"} />
                  </TableRowActions>
                  }
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      {editDialogOpened && editDialog}
    </Fragment>
  );
}
