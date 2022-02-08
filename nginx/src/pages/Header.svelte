<script lang="ts">
    import Card, { Content } from "@smui/card";
    import { Link } from "svelte-navigator";
</script>


<Card>
    <Content class="flex flex-col p-2">
        <Link to="home">Zropbox</Link>
        <Link to="settings">Settings</Link>
    </Content>
</Card>
