<script lang="ts">
    import moment from "moment";

    let classes;
    let displayDate: string = "";

    export let date: Date = new Date();
    export let format: string = "DD MMM Y HH:mm";
    export let title: string = "";
    export { classes as class };

    var locale = window.navigator?.userLanguage || window.navigator.language;

    // set displayDate based on format with moment
    $: displayDate = moment(date).utc(true).utcOffset(locale).format(format);
</script>

<div title={title} class={classes}>{ displayDate }</div>