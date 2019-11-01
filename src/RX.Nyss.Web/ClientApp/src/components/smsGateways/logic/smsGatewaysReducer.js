import * as actions from "./smsGatewaysConstants";
import { initialState } from "../../../initialState";
import { setProperty } from "../../../utils/immutable";

export function smsGatewaysReducer(state = initialState.smsGateways, action) {
  switch (action.type) {
    case actions.GET_SMS_GATEWAYS.REQUEST:
      return { ...state, listFetching: true, listData: [] };

    case actions.GET_SMS_GATEWAYS.SUCCESS:
      return { ...state, listFetching: false, listData: action.list };

    case actions.GET_SMS_GATEWAYS.FAILURE:
      return { ...state, listFetching: false, listData: [] };

    case actions.OPEN_EDITION_SMS_GATEWAY.INVOKE:
      return { ...state, formFetching: true, formData: null };

    case actions.OPEN_EDITION_SMS_GATEWAY.REQUEST:
      return { ...state, formFetching: true, formData: null };

    case actions.OPEN_EDITION_SMS_GATEWAY.SUCCESS:
      return { ...state, formFetching: false, formData: action.data };

    case actions.OPEN_EDITION_SMS_GATEWAY.FAILURE:
      return { ...state, formFetching: false };

    case actions.CREATE_SMS_GATEWAY.REQUEST:
      return { ...state, formSaving: true };

    case actions.CREATE_SMS_GATEWAY.SUCCESS:
      return { ...state, formSaving: false, listData: [] };

    case actions.CREATE_SMS_GATEWAY.FAILURE:
      return { ...state, formSaving: false, formError: action.message };

    case actions.EDIT_SMS_GATEWAY.REQUEST:
      return { ...state, formSaving: true };

    case actions.EDIT_SMS_GATEWAY.SUCCESS:
      return { ...state, formSaving: false, listData: [] };

    case actions.EDIT_SMS_GATEWAY.FAILURE:
      return { ...state, formSaving: false };

    case actions.REMOVE_SMS_GATEWAY.REQUEST:
      return { ...state, listRemoving: setProperty(state.listRemoving, action.id, true) };

    case actions.REMOVE_SMS_GATEWAY.SUCCESS:
    case actions.REMOVE_SMS_GATEWAY.FAILURE:
      return { ...state, listRemoving: setProperty(state.listRemoving, action.id, undefined) };

    default:
      return state;
  }
};
