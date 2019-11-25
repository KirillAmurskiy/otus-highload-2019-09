import React from "react";
import styles from "./baselineGrid.module.css";

function BaselineGrid() {
  return (
    <div className={styles.baselineGrid}>
      <div className={styles.guide}></div>
      <div className={styles.guide}></div>
      <div className={styles.guide}></div>
      <div className={styles.guide}></div>
      <div className={styles.guide}></div>
      <div className={styles.guide}></div>
      <div className={styles.guide}></div>
      <div className={styles.guide}></div>
      <div className={styles.guide}></div>
      <div className={styles.guide}></div>
    </div>
  );
}

export default BaselineGrid;
