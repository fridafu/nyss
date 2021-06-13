import React, {useState} from "react";
import styles from "./AlertEventExpandableText.module.scss";
import KeyboardArrowDownIcon from '@material-ui/icons/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp';
import {IconButton} from "@material-ui/core";

export const AlertEventExpandableText = ({text, maxLength} ) => {

  const [isExpanded, setIsExpanded] = useState(false);

  const isTooLong = text && text.length > maxLength

  const getTruncatedText = () => {
    return text.substring(0, maxLength - 3) + "..."
  }

  const renderShortText = () => {
    return (
      <span>{text}</span>)
  }

  const getText = () => {
    if (!isExpanded) {
      return getTruncatedText()
    }
    return text
  }

  const renderExpandableText = () => {
    return (
      <div className={styles.collapsibleContentWrapper}>
        <div className={styles.collapsibleText}>
          {getText()}
        </div>
        <div className={styles.controls}>
          <IconButton aria-label="expand row" size="small" onClick={ () => setIsExpanded(true)}>
            <KeyboardArrowDownIcon />
          </IconButton>
        </div>
      </div>
    )
  }

  const renderShrinkableText = () => {
    return (
    <div className={styles.collapsibleContentWrapper}>
      <div className={styles.collapsibleText}>
        {getText()}
      </div>
      <div className={styles.controls}>
        <IconButton aria-label="shrink row" size="small" onClick={ () => setIsExpanded(false)}>
          <KeyboardArrowUpIcon />
        </IconButton>
      </div>
    </div>
    )
  }

  if (!isTooLong) {
    return renderShortText()
  }

  if (!isExpanded) {
    return renderExpandableText()
  }

  else {
    return renderShrinkableText()
  }

}